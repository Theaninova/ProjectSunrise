// Copyright (C) 2023  Sunrise Project
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Kaitai;
using SunriseMono.Kaitai;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;

#endregion

namespace SunriseMono.NULib;

public partial class NuModel : GodotObject
{
    public const float ImportScale = 0.1f;

    public System.Collections.Generic.Dictionary<string, ArrayMesh> Meshes = new();

    public string Path;

    public Dictionary<uint, Shader> Shaders =
        new()
        {
            [0x00] = ResourceLoader.Load<Shader>("res://Materials/map_constant.gdshader"), // tire marks (no fog)
            [0x01] = ResourceLoader.Load<Shader>("res://Materials/map_vtx_color.gdshader"), // basic albedo
            [0x15] = ResourceLoader.Load<Shader>("res://Materials/kanban_normal.gdshader"), // arrow boards
            [0x30] = ResourceLoader.Load<Shader>("res://Materials/map_depth.gdshader"), // sunlight
            [0x31] = ResourceLoader.Load<Shader>("res://Materials/map_vertex_color_sc.gdshader"), // mist
            [0xB2] = ResourceLoader.Load<Shader>(
                "res://Materials/map_ref_cube_mt_shadow_mask_add.gdshader"
            ), // lighting
            [0xC2] = ResourceLoader.Load<Shader>(
                "res://Materials/map_ref_cube_shadow_mask_add.gdshader"
            ), // lighting
            [0x91] = ResourceLoader.Load<Shader>(
                "res://Materials/map_vtx_color_mul_blend.gdshader"
            ), // shadow
        };

    public NuModel() { }

    public NuModel(string path, NuTexture[] nuTextures)
        : this(NuCache.Open(path, FileAccess.ModeFlags.Read), nuTextures)
    {
        Path = path;
    }

    public NuModel(Stream stream, NuTexture[] nuTextures)
        : this(new KaitaiStream(stream), nuTextures) { }

    public NuModel(KaitaiStream stream, NuTexture[] nuTextures)
        : this(new Nud(stream), nuTextures) { }

    public NuModel(Nud nud, NuTexture[] nuTextures)
    {
        foreach (var nudMesh in nud.Meshes)
        {
            var mesh = new ArrayMesh();

            foreach (var part in nudMesh.Parts)
            {
                var surfaceArray = new Array();
                surfaceArray.Resize((int)Mesh.ArrayType.Max);

                surfaceArray[(int)Mesh.ArrayType.Vertex] = (
                    from vertex in part.Vertices
                    select new Vector3(
                        vertex.Position[0] * ImportScale,
                        vertex.Position[1] * ImportScale,
                        vertex.Position[2] * ImportScale
                    )
                ).ToArray();
                surfaceArray[(int)Mesh.ArrayType.Index] = GetRenderingVertexIndices(
                        part.Indices,
                        part.PolySize
                    )
                    .ToArray();
                surfaceArray[(int)Mesh.ArrayType.TexUV] = (
                    from vertex in part.Vertices
                    select new Vector2(
                        ConvertUv(vertex.Uv[0], part.UvFloat),
                        ConvertUv(vertex.Uv[1], part.UvFloat)
                    )
                ).ToArray();
                if (part.UvChannelCount > 1)
                    surfaceArray[(int)Mesh.ArrayType.TexUV2] = (
                        from vertex in part.Vertices
                        select new Vector2(
                            ConvertUv(vertex.Uv[2], part.UvFloat),
                            ConvertUv(vertex.Uv[3], part.UvFloat)
                        )
                    ).ToArray();

                if (part.UvChannelCount > 2)
                    GD.PushError("Only 2 UV channels are supported");

                surfaceArray[(int)Mesh.ArrayType.Normal] = (
                    from vertex in part.Vertices
                    select new Vector3(
                        ConvertNormal(vertex.Normal[0], part.NormalHalfFloat),
                        ConvertNormal(vertex.Normal[1], part.NormalHalfFloat),
                        ConvertNormal(vertex.Normal[2], part.NormalHalfFloat)
                    )
                ).ToArray();
                surfaceArray[(int)Mesh.ArrayType.Color] = (
                    from vertex in part.Vertices
                    select new Color(
                        vertex.Colors[0],
                        vertex.Colors[1],
                        vertex.Colors[2],
                        vertex.Colors[3]
                    )
                ).ToArray();

                mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);

                if (part.Materials.Count > 1)
                    GD.PushWarning("Multiple materials ", nudMesh.Name);

                if (part.Materials.Count > 0)
                {
                    var nudMaterial = part.Materials[0].Material;
                    var material = new ShaderMaterial();
                    if (!Shaders.TryGetValue(nudMaterial.Flags, out var shader) || shader == null)
                    {
                        GD.PrintErr($"Missing Shader for {nudMesh.Name} {{");
                        GD.PrintErr(
                            $"  flags:   0x{nudMaterial.Flags:X2};\n"
                                + $"  fog:     0x{nudMaterial.Fog:X2};\n"
                                + $"  layers:  0x{nudMaterial.TexLayers:X2};\n"
                                + $"  effects: 0x{nudMaterial.TexLayers:X2};\n"
                                + $"  tex:     {nudMaterial.NumMaterialTextures}\n"
                                + $"  attr: {{\n"
                                + string.Join(
                                    "",
                                    nudMaterial.MaterialAttributes.Select(
                                        it =>
                                            $"    {it.Name}: [{string.Join(", ", it.Values.Take((int)it.NumValues))}];\n"
                                    )
                                )
                                + "  }\n"
                                + "}"
                        );
                        continue;
                    }

                    material.Shader = shader;
                    var uniforms = shader
                        .GetShaderUniformList()
                        .Select(it => it.AsGodotDictionary())
                        .ToDictionary(
                            it => it["name"].AsString(),
                            it =>
                                (
                                    Type: it["type"].As<Variant.Type>(),
                                    Hint: it["hint_string"].AsString()
                                )
                        );

                    foreach (
                        var attribute in nudMaterial.MaterialAttributes.Where(
                            attribute => attribute.Name != null
                        )
                    )
                    {
                        if (!uniforms.TryGetValue(attribute.Name, out var info))
                        {
                            GD.PushError(
                                $"Missing attribute '{attribute.Name}' in {shader.ResourcePath}"
                            );
                        }

                        switch (attribute.NumValues)
                        {
                            case 1:
                                if (info.Type != Variant.Type.Float)
                                    GD.PushError(
                                        $"Attribute {attribute.Name} must have type float ({shader.ResourcePath})"
                                    );
                                material.SetShaderParameter(attribute.Name, attribute.Values[0]);
                                break;
                            case 2:
                                if (info.Type != Variant.Type.Vector2)
                                    GD.PushError(
                                        $"Attribute {attribute.Name} must have type vec2 ({shader.ResourcePath})"
                                    );
                                material.SetShaderParameter(
                                    attribute.Name,
                                    new Vector2(attribute.Values[0], attribute.Values[1])
                                );
                                break;
                            case 3:
                                if (info.Type != Variant.Type.Vector3)
                                    GD.PushError(
                                        $"Attribute {attribute.Name} must have type vec3 ({shader.ResourcePath})"
                                    );
                                material.SetShaderParameter(
                                    attribute.Name,
                                    new Vector3(
                                        attribute.Values[0],
                                        attribute.Values[1],
                                        attribute.Values[2]
                                    )
                                );
                                break;
                            case 4:
                                if (info.Type != Variant.Type.Vector4)
                                    GD.PushError(
                                        $"Attribute {attribute.Name} must have type vec4 ({shader.ResourcePath})"
                                    );
                                material.SetShaderParameter(
                                    attribute.Name,
                                    new Vector4(
                                        attribute.Values[0],
                                        attribute.Values[1],
                                        attribute.Values[2],
                                        attribute.Values[3]
                                    )
                                );
                                break;
                            default:
                                GD.Print(
                                    $"Unknown attribute count {attribute.NumValues} ({shader.ResourcePath})"
                                );
                                break;
                        }
                    }

                    // if (nudMaterial.DstFactor != 0 || nudMaterial.SrcFactor != 0)
                    // {
                    //     GD.Print($"{nudMesh.Name}: src={nudMaterial.SrcFactor}, dst={nudMaterial.DstFactor}");
                    //     if (nudMaterial.SrcFactor == 15 && nudMaterial.DstFactor == 10)
                    //         material.BlendMode = BaseMaterial3D.BlendModeEnum.Mul;
                    //     else if (nudMaterial.SrcFactor == 15 && nudMaterial.DstFactor == 1)
                    //         material.BlendMode = BaseMaterial3D.BlendModeEnum.Add;
                    // }

                    // foreach (var attribute in nudMaterial.MaterialAttributes)
                    // {
                    //     if (attribute.Name is null or "NU_FOGPOWER") continue;
                    //     GD.Print($"{attribute.Name}: {attribute.Values.ToArray().Join(", ")}");
                    //     if (attribute.Name is "NU_COMA_MAX" or "NU_SCROLLSPEED1")
                    //         material.BlendMode = BaseMaterial3D.BlendModeEnum.Mix;
                    // }

                    // material.RenderPriority = (int)Material.RenderPriorityMax - nudMaterial.MaterialType;


                    var texIndex = 0;
                    var cubemapIndex = 0;
                    foreach (var texture in nudMaterial.MaterialTextures)
                    {
                        foreach (var nuTexture in nuTextures)
                        {
                            if (nuTexture.Textures.TryGetValue(texture.Hash, out var imageTexture))
                            {
                                var name = $"custom_texture{texIndex:D2}";
                                if (!uniforms.TryGetValue(name, out var info))
                                    GD.PushError($"Missing texture {name} ({shader.ResourcePath})");
                                material.SetShaderParameter(name, imageTexture);
                                texIndex++;
                                break;
                            }

                            if (nuTexture.Cubemaps.TryGetValue(texture.Hash, out var cubemap))
                            {
                                var name = $"custom_cubemap{cubemapIndex:D2}";
                                if (!uniforms.TryGetValue(name, out var info))
                                    GD.PushError($"Missing cubemap {name} ({shader.ResourcePath})");
                                material.SetShaderParameter(name, cubemap);
                                cubemapIndex++;
                                break;
                            }
                        }
                    }

                    mesh.SurfaceSetMaterial(mesh.GetSurfaceCount() - 1, material);
                }
            }

            Meshes[nudMesh.Name] = mesh;
        }
    }

    public void SaveToCache()
    {
        var baseDir = NuCache.ReplacePrefix(NuCache.CachePrefix, Path.GetBaseName());
        NuCache.MakeDirRecursive(baseDir);

        var i = 0;
        foreach (var (name, mesh) in Meshes)
            NuCache.Save(mesh, $"{baseDir}/{(name.IsValidFileName() ? name : i++)}.res");
    }

    private static float ConvertUv(double uv, bool uvFloat)
    {
        return uvFloat ? (float)uv : (float)BitConverter.ToHalf(BitConverter.GetBytes((ushort)uv));
    }

    private static float ConvertNormal(double normal, bool normalHalfFloat)
    {
        return ConvertUv(normal, !normalHalfFloat);
    }

    private static List<int> GetRenderingVertexIndices(IReadOnlyList<ushort> strip, byte polySize)
    {
        if (polySize >> 4 == 4)
            return strip.Cast<int>().ToList();

        var vertexIndices = new List<int>();

        var startDirection = -1;
        var p = 0;
        int f1 = strip[p++];
        int f2 = strip[p++];
        var faceDirection = startDirection;
        int f3;
        do
        {
            f3 = strip[p++];
            if (f3 == 0xFFFF)
            {
                f1 = strip[p++];
                f2 = strip[p++];
                faceDirection = startDirection;
            }
            else
            {
                faceDirection *= -1;
                if (f1 != f2 && f2 != f3 && f3 != f1)
                {
                    if (faceDirection > 0)
                    {
                        vertexIndices.Add(f3);
                        vertexIndices.Add(f2);
                        vertexIndices.Add(f1);
                    }
                    else
                    {
                        vertexIndices.Add(f2);
                        vertexIndices.Add(f3);
                        vertexIndices.Add(f1);
                    }
                }

                f1 = f2;
                f2 = f3;
            }
        } while (p < strip.Count);

        return vertexIndices;
    }
}
