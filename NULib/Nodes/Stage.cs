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
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Kaitai;
using SunriseMono.NULib.Lua;
using Environment = Godot.Environment;
using FileAccess = Godot.FileAccess;

namespace SunriseMono.NULib.Nodes;

[Tool]
public partial class Stage : Enma3D
{
    public enum StageTime
    {
        Day = 0,
        Ngt = 1,
    }

    public enum StageVariant
    {
        Nml = 0,
        Ext = 1,
    }

    public enum AreaEnum
    {
        Tokyo = 0,
        Hakone = 1,
        Nagoya = 2,
        Osaka = 3,
        Fukuoka = 4,
        Ikebukro = 5, // these two
        Shibuya = 6, // might be swapped
        Turnpike = 7,
        Kobe = 8,
        Story = 9
    }

    [Export] public AreaEnum Area = AreaEnum.Hakone;
    [Export] public string StageName = "HAKONE";
    [Export] public StageTime Time = StageTime.Day;
    [Export] public StageVariant Variant = StageVariant.Nml;

    [Export] public bool AsyncLoading = false;

    [Export]
    public bool Reload
    {
        get => false;
        set
        {
            if (!value) return;
            GD.PushWarning("Reloading...");
            if (AsyncLoading) Task.Run(Load);
            else Load();
        }
    }

    public string AreaString
    {
        get
        {
            var areaString = Enum.GetName(typeof(AreaEnum), Area)?.ToUpper();
            var timeString = Enum.GetName(typeof(StageTime), Time)?.ToUpper();
            var variantString = Enum.GetName(typeof(StageVariant), Variant)?.ToUpper();

            return $"A_{areaString}_{timeString}_{variantString}";
        }
    }

    public string StagePath => $"game://data/course/stage/{AreaString}/{StageName}";

    public override void _Ready()
    {
        if (AsyncLoading) Task.Run(Load);
        else Load();
    }

    private void Load()
    {
        try
        {
            foreach (var child in GetChildren())
                RemoveChild(child);

            if (StageName == "COMMON")
            {
                LoadSkybox();
            }
            else
            {
                LoadStage();
            }
        }
        catch (Exception e)
        {
            GD.PushError($"Failed to load stage: {e}\n{e.StackTrace}");
        }
    }

    private void LoadStage()
    {
        
        var paramAccessor = new ParamAccessor();
        var stageAccessor = new StageAccessor(StagePath);

        // Environment
        // {
        //     var worldEnvironment = new WorldEnvironment();
        //     worldEnvironment.Name = "Environment";
        //     worldEnvironment.Environment = ResourceLoader.Load<Environment>("res://track_environment.tres");
        //     worldEnvironment.Environment.AmbientLightColor = paramAccessor.GetGlobalLight_Ambient(Area, Time);
        //     worldEnvironment.Environment.BackgroundColor = paramAccessor.GetGlobalLight_Ambient(Area, Time);
        //     worldEnvironment.Environment.BackgroundMode = Environment.BGMode.Color;
        // 
        //     AddChild(worldEnvironment);
        // }

        // Global Light
        {
            var light = new DirectionalLight3D();
            light.Name = "GlobalLight";
            // light.ShadowEnabled = true;
            // light.DirectionalShadowSplit1 = 0.08f;
            // light.DirectionalShadowMaxDistance = 250f;
            // light.LightVolumetricFogEnergy = 4;
            var (position, pitch) = paramAccessor.GetGlobalLight_Position(Area, Time);
            position.Y = 0;
            light.Quaternion = new Quaternion(position.Normalized(), -pitch);
            light.LightColor = paramAccessor.GetGlobalLight_Color(Area, Time);
            AddChild(light);
        }

        AddChild(LoadStageModels(stageAccessor));
        paramAccessor.Dispose();
        stageAccessor.Dispose();
    }

    private void LoadSkybox()
    {
        var areaAccessor = new AreaAccessor(StagePath);
        AddChild(LoadAreaModels(areaAccessor));
        areaAccessor.Dispose();
    }

    private void LoadModels(
        IEnumerable<(string, AddrName[])> relevantModels,
        Node parent,
        NuTexture[] textures,
        Stream binStream)
    {
        foreach (var (category, models) in relevantModels)
        {
            var categoryRoot = new Node3D();
            categoryRoot.Name = category;
            parent.AddChild(categoryRoot);

            foreach (var address in models)
            {
                var bytes = new byte[address.Length];
                binStream.Seek(address.Start, SeekOrigin.Begin);
                binStream.Read(bytes, 0, address.Length);

                var model = new NuModel(new KaitaiStream(bytes), textures);
                foreach (var (partName, part) in model.Meshes)
                {
                    // if (partName.Contains("shadow")) continue;
                    var partInstance = new MeshInstance3D();
                    partInstance.Name = partName;
                    partInstance.Mesh = part;
                    categoryRoot.AddChild(partInstance);
                }
            }
        }
    }

    private Node3D LoadAreaModels(AreaAccessor areaAccessor)
    {
        var textures = (
            from texture in areaAccessor.TextureNames
            select new NuTexture($"{StagePath}/model/nut/{texture}")
        ).ToArray();
        
        var root = new Node3D();
        root.Name = "Background";

        var binPath = $"{StagePath}/model/bin/{areaAccessor.BinPath.GetFile()}";
        var binStream = NuCache.Open(binPath, FileAccess.ModeFlags.Read);
        
        var relevantModels = new[]
        {
            ("Skybox", areaAccessor.Skybox),
            ("Residential", areaAccessor.Residential),
        };

        LoadModels(relevantModels, root, textures, binStream);

        return root;
    }

    private Node3D LoadStageModels(StageAccessor stageAccessor)
    {
        var textures = (
            from texture in stageAccessor.TextureNames
            select new NuTexture($"{StagePath}/model/nut/{texture}")
        ).ToArray();
        var sectionCount = stageAccessor.SectionCount;

        var root = new Node3D();
        root.Name = Name;

        for (var section = 1; section <= sectionCount; section++)
        {
            var binPath = $"{StagePath}/model/bin/{stageAccessor.GetBinPath(section).GetFile()}";
            var binStream = NuCache.Open(binPath, FileAccess.ModeFlags.Read);

            var sectionRoot = new Node3D();
            sectionRoot.Name = stageAccessor.GetBinPath(section).GetBaseName().GetFile();
            root.AddChild(sectionRoot);

            var relevantModels = new[]
            {
                ("Road", stageAccessor.GetRoad(section)),
                ("On Road", stageAccessor.GetOnRoad(section)),
                ("Near View", stageAccessor.GetNearView(section)),
                ("Cast", stageAccessor.GetCast(section)), // ?
                ("Long Range", stageAccessor.GetLongRangeDisplay(section)), // ?
            };

            LoadModels(relevantModels, sectionRoot, textures, binStream);
        }

        return root;
    }
}