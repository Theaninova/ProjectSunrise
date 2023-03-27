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
using System.Linq;
using System.Resources;
using Godot;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Lumen;

public static class LumenSprite
{
    public static PackedScene ToControl(this LumenTag<Lmd.DefineSprite> self)
    {
        var container = new Control();
        var scene = new PackedScene();
        var name = self.Flash.Symbols[(int)self.Tag.NameId];
        container.Name = name == "" ? self.Tag.CharacterId.ToGdCharacterId() : name;

        if (!self.Children.Any())
        {
            scene.Pack(container);
            return scene;
        }

        var animation = new Animation();
        const string animationName = "show_frame";
        animation.Length = self.Children.Count / self.Flash.FrameRate;
        animation.Step = 1 / self.Flash.FrameRate;
        // TODO: replace once actions are implemented
        animation.LoopMode = Animation.LoopModeEnum.Linear;

        var library = new AnimationLibrary();
        const string libraryName = "sprite";
        library.AddAnimation(animationName, animation);

        var player = new AnimationPlayer();
        player.Name = "Animations";
        player.AddAnimationLibrary(libraryName, library);
        // TODO: replace once actions are implemented
        player.Autoplay = $"{libraryName}/{animationName}";

        container.AddChild(player);
        player.Owner = container;

        var objectStack = new Dictionary<int, Control>();
        var tracks = new Dictionary<int, LumenAnimation.LumenAnimationTrackKeys>();
        var actionTracks = new Dictionary<int, int>();

        // TODO: keyframes/frames optimization
        // foreach (var keyframe in self.FindChildren<Lmd.Frame>(Lmd.Tag.FlashTagType.Keyframe))
        // {
        //     GD.PushError("Keyframes are not implemented");
        // }
        foreach (var frame in self.FindChildren<Lmd.Frame>().OrderBy(it => it.Tag.Id))
        {
            var frameIndex = frame.Tag.Id;
            var keyTime = frameIndex / self.Flash.FrameRate;

            foreach (var child in frame.Children)
            {
                if (child.TryToLumenTag<Lmd.DoAction>(self.Flash, out var doAction))
                {
                    // TODO...
                    continue;
                }

                child.TryToLumenTag<Lmd.PlaceObject>(self.Flash, out var placeObject);
                child.TryToLumenTag<Lmd.RemoveObject>(self.Flash, out var removeObject);
                if (placeObject == null && removeObject == null)
                {
                    GD.PushError(
                        $"Unknown frame child type 0x{child.TagType:X} in {self.Tag.CharacterId}/{frameIndex - 1}"
                    );
                    continue;
                }

                var depth = placeObject != null ? placeObject.Tag.Depth : removeObject.Tag.Depth;
                if (!objectStack.TryGetValue(depth, out var target))
                {
                    if (placeObject == null)
                        throw new Exception("Can't remove object that doesn't exist");
                    var defineScene = self.Flash.Defines[(uint)placeObject.Tag.CharacterId];
                    
                    target = defineScene.Instantiate<Control>();
                    objectStack[depth] = target;
                }

                if (!tracks.TryGetValue(depth, out var keys))
                {
                    keys = animation.InsertLumenTracks(target);
                    tracks[depth] = keys;
                }

                if (removeObject != null)
                    animation.TrackInsertKey(keys.Visibility, keyTime, false);
                if (placeObject != null)
                    animation.TrackInsertObjectPlacementKeys(keys, keyTime, placeObject);
            }
        }

        foreach (var (_, target) in objectStack.OrderBy(pair => pair.Key))
        {
            container.AddChild(target);
            target.Owner = container;
        }
        
        foreach (var (depth, keys) in tracks)
        {
            var mergedPositionTrack = animation.ValueTrackMergeTracks<Vector2>(keys.Position, keys.Translate);
            animation.TrackClearKeys(keys.Position);
            animation.TrackClearKeys(keys.Translate);

            animation.TrackFoldConstant<Vector2>(mergedPositionTrack, value => objectStack[depth].Position = value);
            animation.TrackFoldConstant<bool>(keys.Visibility, value => objectStack[depth].Visible = value);
            animation.TrackFoldConstant<double>(keys.Rotate, value => objectStack[depth].Rotation = (float)value);
            animation.TrackFoldConstant<Vector2>(keys.Scale, value => objectStack[depth].Scale = value);
            animation.TrackFoldConstant<Color>(keys.Modulate, value => objectStack[depth].Modulate = value);
        }
        animation.CleanupEmptyTracks();

        if (animation.GetTrackCount() == 0)
        {
            library.RemoveAnimation(animationName);
            container.RemoveChild(player);
            player.Dispose();
        }

        scene.Pack(container);
        return scene;
    }
}