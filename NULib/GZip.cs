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
using System.IO;
using System.IO.Compression;

#endregion

namespace SunriseMono.NULib;

public class GZip
{
    public static Stream MaybeCompressedFromFile(string path)
    {
        if (!File.Exists(path))
            throw new Exception("Unknown file");
        var stream = new StreamReader(path);
        Stream decompressedStream = new MemoryStream();
        try
        {
            var gz = new GZipStream(stream.BaseStream, CompressionMode.Decompress);
            gz.CopyTo(decompressedStream);
        }
        catch
        {
            decompressedStream = new StreamReader(path).BaseStream;
        }

        decompressedStream.Seek(0, SeekOrigin.Begin);
        return decompressedStream;
    }
}
