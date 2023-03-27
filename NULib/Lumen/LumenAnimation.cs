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
using Godot;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Lumen;

public static class LumenAnimation
{
    public record LumenAnimationTrackKeys(
        int Position,
        int Visibility,
        int Translate,
        int Rotate,
        int Scale,
        int Modulate
    );

    public static void TrackFoldConstant<[MustBeVariant] TOut>(
        this Animation animation,
        int idx,
        Action<TOut> apply
    )
    {
        if (animation.TrackGetKeyCount(idx) < 1)
            return;

        var last = animation.TrackGetKeyValue(idx, 0).As<TOut>();
        for (var i = 1; i < animation.TrackGetKeyCount(idx); i++)
        {
            var value = animation.TrackGetKeyValue(idx, i).As <TOut>();
            if (!last.Equals(value))
                return;

            last = value;
        }
        animation.TrackClearKeys(idx);

        apply(last);
    }

    public static void CleanupEmptyTracks(this Animation animation)
    {
        var tracksToRemove = new List<int>();
        for (var i = 0; i < animation.GetTrackCount(); i++)
        {
            if (animation.TrackGetKeyCount(i) != 0)
                continue;
            tracksToRemove.Add(i);
        }

        tracksToRemove.Sort();
        tracksToRemove.Reverse();
        foreach (var idx in tracksToRemove)
            animation.RemoveTrack(idx);
    }

    public static int ValueTrackMergeTracks<[MustBeVariant] TKey>(this Animation animation, params int[] indices)
    {
        var times = new HashSet<double>();

        foreach (var trackIdx in indices)
            foreach (var i in Enumerable.Range(0, animation.TrackGetKeyCount(trackIdx)))
                times.Add(animation.TrackGetKeyTime(trackIdx, i));

        var virtualTrack = animation.AddTrack(Animation.TrackType.Value);
        animation.TrackSetPath(virtualTrack, animation.TrackGetPath(indices[0]));
        animation.ValueTrackSetUpdateMode(virtualTrack, animation.ValueTrackGetUpdateMode(indices[0]));

        foreach (var time in times)
            animation.TrackInsertKey(
                virtualTrack,
                time,
                Variant.From(indices
                    .Select(it => animation.ValueTrackInterpolate(it, time).As<TKey>())
                    .Aggregate((acc, curr) => (dynamic)acc + (dynamic)curr))
            );

        return virtualTrack;
    }

    public static void TrackClearKeys(this Animation animation, int idx)
    {
        foreach (var i in Enumerable.Range(0, animation.TrackGetKeyCount(idx)).Reverse())
            animation.TrackRemoveKey(idx, i);
    }

    public static void TrackInsertObjectPlacementKeys(
        this Animation animation,
        LumenAnimationTrackKeys keys,
        float keyTime,
        LumenTag<Lmd.PlaceObject> placeObject
    )
    {
        if (placeObject == null)
            return;
        if (placeObject.Tag.PlacementMode == Lmd.PlacementMode.Place)
            animation.TrackInsertKey(keys.Visibility, keyTime, true);

        switch (placeObject.Tag.PositionFlags)
        {
            case Lmd.PositionFlags.Position:
                var position = placeObject.Flash.Positions[placeObject.Tag.PositionId];
                animation.TrackInsertKey(keys.Position, keyTime, position);
                break;
            case Lmd.PositionFlags.Transform:
                var transform = placeObject.Flash.Transforms[placeObject.Tag.PositionId];
                animation.TrackInsertKey(keys.Translate, keyTime, transform.Origin);
                animation.TrackInsertKey(keys.Rotate, keyTime, transform.Rotation);
                animation.TrackInsertKey(keys.Scale, keyTime, transform.Scale);
                break;
            case Lmd.PositionFlags.NoTransform:
                break;
            default:
                throw new NotImplementedException();
        }

        if (placeObject.Tag.ColorMultId == -1)
            return;
        var color = placeObject.Flash.Colors[placeObject.Tag.ColorMultId];
        animation.TrackInsertKey(keys.Modulate, keyTime, color);
    }

    public static LumenAnimationTrackKeys InsertLumenTracks(this Animation animation, Node target)
    {
        return new LumenAnimationTrackKeys(
            animation.AddTrack(Animation.TrackType.Value, target, "position"),
            animation.AddTrack(Animation.TrackType.Value, target, "visible"),
            animation.AddTrack(Animation.TrackType.Value, target, "position"),
            animation.AddTrack(Animation.TrackType.Value, target, "rotation"),
            animation.AddTrack(Animation.TrackType.Value, target, "scale"),
            animation.AddTrack(Animation.TrackType.Value, target, "modulate")
        );
    }

    private static int AddTrack(
        this Animation animation,
        Animation.TrackType type,
        Node target,
        string name
    )
    {
        var idx = animation.AddTrack(type);
        animation.TrackSetPath(idx, $"{target.Name}:{name}");
        animation.ValueTrackSetUpdateMode(idx, Animation.UpdateMode.Discrete);
        return idx;
    }
}
