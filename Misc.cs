﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Imaging;

namespace ManicDigger
{
    public static class GameVersion
    {
        static string gameversion;
        public static string Version
        {
            get
            {
                if (gameversion == null)
                {
                    gameversion = "unknown";
                    if (File.Exists("version.txt"))
                    {
                        gameversion = File.ReadAllText("version.txt");
                    }
                }
                return gameversion;
            }
        }
    }
    public static class GzipCompression
    {
        public static byte[] Compress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (GZipStream compress = new GZipStream(output, CompressionMode.Compress))
            {
                byte[] buffer = new byte[4096];
                int numRead;
                while ((numRead = input.Read(buffer, 0, buffer.Length)) != 0)
                {
                    compress.Write(buffer, 0, numRead);
                }
            }
            return output.ToArray();
        }
        public static byte[] Decompress(byte[] fi)
        {
            MemoryStream ms = new MemoryStream();
            // Get the stream of the source file.
            using (MemoryStream inFile = new MemoryStream(fi))
            {
                // Get original file extension, for example "doc" from report.doc.gz.
                //string curFile = fi.FullName;
                //string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                //Create the decompressed file.
                //using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,
                            CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, numRead);
                        }
                        //Console.WriteLine("Decompressed: {0}", fi.Name);
                    }
                }
            }
            return ms.ToArray();
        }
        public static byte[] Decompress(FileInfo fi)
        {
            MemoryStream ms = new MemoryStream();
            // Get the stream of the source file.
            using (FileStream inFile = fi.OpenRead())
            {
                // Get original file extension, for example "doc" from report.doc.gz.
                string curFile = fi.FullName;
                string origName = curFile.Remove(curFile.Length - fi.Extension.Length);

                //Create the decompressed file.
                //using (FileStream outFile = File.Create(origName))
                {
                    using (GZipStream Decompress = new GZipStream(inFile,
                            CompressionMode.Decompress))
                    {
                        //Copy the decompression stream into the output file.
                        byte[] buffer = new byte[4096];
                        int numRead;
                        while ((numRead = Decompress.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            ms.Write(buffer, 0, numRead);
                        }
                        //Console.WriteLine("Decompressed: {0}", fi.Name);
                    }
                }
            }
            return ms.ToArray();
        }
    }
    public class FastBitmap
    {
        public Bitmap bmp;
        BitmapData bmd;
        public void Lock()
        {
            if (bmd != null)
            {
                throw new Exception("Already locked.");
            }
            if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb)
            {
                throw new Exception();
            }
            bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
        }
        public int GetPixel(int x, int y)
        {
            if (bmd == null)
            {
                throw new Exception();
            }
            unsafe
            {
                int* row = (int*)((byte*)bmd.Scan0 + (y * bmd.Stride));
                return row[x];
            }
        }
        public void SetPixel(int x, int y, int color)
        {
            if (bmd == null)
            {
                throw new Exception();
            }
            unsafe
            {
                int* row = (int*)((byte*)bmd.Scan0 + (y * bmd.Stride));
                row[x] = color;
            }
        }
        public void Unlock()
        {
            if (bmd == null)
            {
                throw new Exception("Not locked.");
            }
            bmp.UnlockBits(bmd);
            bmd = null;
        }
    }
    public struct Vector2i
    {
        public Vector2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x;
        public int y;
        public override bool Equals(object obj)
        {
            if (obj is Vector2i)
            {
                Vector2i other = (Vector2i)obj;
                return this.x == other.x && this.y == other.y;
            }
            return base.Equals(obj);
        }
        public static bool operator ==(Vector2i a, Vector2i b)
        {
            return a.x == b.x && a.y == b.y;
        }
        public static bool operator !=(Vector2i a, Vector2i b)
        {
            return !(a.x == b.x && a.y == b.y);
        }
        public override int GetHashCode()
        {
            int hash = 23;
            unchecked
            {
                hash = hash * 37 + x;
                hash = hash * 37 + y;
            }
            return hash;
        }
    }
    public struct Vector3i
    {
        public Vector3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public int x;
        public int y;
        public int z;
        public override bool Equals(object obj)
        {
            if (obj is Vector3i)
            {
                Vector3i other = (Vector3i)obj;
                return this.x == other.x && this.y == other.y && this.z == other.z;
            }
            return base.Equals(obj);
        }
        public static bool operator ==(Vector3i a, Vector3i b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3i a, Vector3i b)
        {
            return !(a.x == b.x && a.y == b.y && a.z == b.z);
        }
        public override int GetHashCode()
        {
            int hash = 23;
            unchecked
            {
                hash = hash * 37 + x;
                hash = hash * 37 + y;
                hash = hash * 37 + z;
            }
            return hash;
        }
    }
    public class DependencyChecker
    {
        public DependencyChecker()
        {
        }
        public DependencyChecker(params Type[] injectAttributes)
        {
            this.InjectAttributes = injectAttributes;
        }
        [Inject]
        public Type[] InjectAttributes { get; set; }
        public void CheckDependencies(params object[] components)
        {
            if (InjectAttributes == null || InjectAttributes.Length == 0)
            {
                throw new Exception("Inject attributes list is null.");
            }
            foreach (object o in components)
            {
                CheckDependencies1(o);
            }
        }
        private void CheckDependencies1(object o)
        {
            Type type = o.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes(true);
                foreach (var a in attributes)
                {
                    if (a is InjectAttribute)
                    {
                        if (property.GetValue(o, null) == null)
                        {
                            throw new Exception(string.Format("Dependency {0} of object of type {1} is null.", property.Name, type.Name));
                        }
                    }
                }
            }
        }
    }
}
