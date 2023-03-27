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

using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using NLua;
using Array = Godot.Collections.Array;
using FileAccess = Godot.FileAccess;

namespace SunriseMono.NULib;

/**
 * A LUA-based surface
 */
public class NuSurface
{
    public NuSurface(string content)
    {
    }

    public static Node3D LoadEnvParam(string path)
    {
        var state = new NLua.Lua();

        foreach (var file in NuCache.GetFilesAt(path))
        {
            if (file.GetExtension().ToLower() != "lua") continue;
            state.DoString(new StreamReader(NuCache.Open($"{path}/{file}", FileAccess.ModeFlags.Read)).ReadToEnd());
        }

        var sceneRoot = new Node3D();
        sceneRoot.Name = "Env Param";

        foreach (var name in new[] { "EFFECT", "DISTCLIP", "HIDECLIP" })
        {
            var surface = (LuaTable)((LuaTable)state[name])["SURFACE"];
            var effectNode = new Node3D();
            effectNode.Name = name;
            sceneRoot.AddChild(effectNode);

            foreach (LuaTable section in surface.Values)
            {
                var sectionId = ((int)(long)section["SECT_ID"]).ToString();
                var sectionNode = effectNode.FindChild(sectionId);
                if (sectionNode == null)
                {
                    sectionNode = new Node3D();
                    sectionNode.Name = sectionId;
                    effectNode.AddChild(sectionNode);
                }

                foreach (LuaTable param in ((LuaTable)section["PARAM"]).Values)
                {
                    var node = new MeshInstance3D();
                    node.Name = (string)param["PARAM_FNAME"];
                    node.Mesh = FromGeo((LuaTable)param["GEO"]);
                    sectionNode.AddChild(node);
                }
            }
        }

        {
            var surface = (LuaTable)((LuaTable)state["SHADOW"])["SURFACE"];
            var effectNode = new Node3D();
            effectNode.Name = "SHADOW";
            sceneRoot.AddChild(effectNode);

            foreach (LuaTable section in surface.Values)
            {
                var sectionId = ((int)(long)section["SECT_ID"]).ToString();

                var sectionNode = new MeshInstance3D();
                sectionNode.Name = sectionId;
                sectionNode.Mesh = FromGeo((LuaTable)section["GEO"]);
                effectNode.AddChild(sectionNode);
            }
        }

        state.Dispose();

        return sceneRoot;
    }

    public static ArrayMesh FromGeo(LuaTable table)
    {
        var mesh = new ArrayMesh();
        var surfaceArray = new Array();
        surfaceArray.Resize((int)Mesh.ArrayType.Max);

        var vertices = new List<Vector3>();
        var indices = new List<int>();

        var vertexCache = new System.Collections.Generic.Dictionary<Vector3, int>();

        var i = 0;
        foreach (LuaTable tri in table.Values)
        {
            for (var j = 2; j >= 0; j--)
            {
                var x = (float)(double)tri[j * 6 + 1];
                var y = (float)(double)tri[j * 6 + 2];
                var z = (float)(double)tri[j * 6 + 3];
                var v = new Vector3(x, y, z);
                if (vertexCache.TryGetValue(v, out var index))
                {
                    indices.Add(index);
                }
                else
                {
                    vertices.Add(v);
                    vertexCache.Add(v, i);
                    indices.Add(i);
                    i++;
                }
            }
        }

        surfaceArray[(int)Mesh.ArrayType.Vertex] = vertices.ToArray();
        surfaceArray[(int)Mesh.ArrayType.Index] = indices.ToArray();

        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
        var mat = new StandardMaterial3D();
        mat.CullMode = BaseMaterial3D.CullModeEnum.Disabled;
        mesh.SurfaceSetMaterial(0, mat);

        return mesh;
    }
}