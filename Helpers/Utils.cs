﻿#region using directives

using System;
using System.Collections.Generic;
using System.Data.HashFunction;
using System.Linq;

#endregion

namespace PokemonGo.RocketAPI.Helpers
{
    public class Utils
    {
        public static ulong FloatAsUlong(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static long GetTime(bool ms = false)
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1);

            if (ms)
                return (long)Math.Round(timeSpan.TotalMilliseconds);
            return (long)Math.Round(timeSpan.TotalSeconds);
        }

        public static uint GenerateLocation1(byte[] authTicket, double lat, double lng, double alt)
        {
            var seed = BitConverter.ToUInt32(new xxHash(32, 0x1B845238).ComputeHash(authTicket), 0);
            var xxh32 = new xxHash(32, seed);

            var locationBytes = new List<byte>();
            locationBytes.AddRange(BitConverter.GetBytes(lat).Reverse());
            locationBytes.AddRange(BitConverter.GetBytes(lng).Reverse());
            locationBytes.AddRange(BitConverter.GetBytes(alt).Reverse());

            return BitConverter.ToUInt32(xxh32.ComputeHash(locationBytes.ToArray()), 0);
        }

        public static uint GenerateLocation2(double lat, double lng, double alt)
        {
            var xxh32 = new xxHash(32, 0x1B845238);

            var locationBytes = new List<byte>();
            locationBytes.AddRange(BitConverter.GetBytes(lat).Reverse());
            locationBytes.AddRange(BitConverter.GetBytes(lng).Reverse());
            locationBytes.AddRange(BitConverter.GetBytes(alt).Reverse());

            return BitConverter.ToUInt32(xxh32.ComputeHash(locationBytes.ToArray()), 0);
        }

        public static ulong GenerateRequestHash(byte[] authTicket, byte[] request)
        {
            var seed = BitConverter.ToUInt64(new xxHash(64, 0x1B845238).ComputeHash(authTicket), 0);
            var xxh64 = new xxHash(64, seed);
            return BitConverter.ToUInt64(xxh64.ComputeHash(request), 0);
        }
    }
}