﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
// Esta rutina fue extraida de https://gist.github.com/bradsjm/84b2d029bbcae6cb5237


namespace uuIdClass
{
    public static class Uuid
    {
        /// <summary>
        ///     The namespace for fully-qualified domain names (from RFC 4122, Appendix C).
        /// </summary>
        public static readonly Guid DnsNamespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

        /// <summary>
        ///     The namespace for URLs (from RFC 4122, Appendix C).
        /// </summary>
        public static readonly Guid UrlNamespace = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8");

        /// <summary>
        ///     The namespace for ISO OIDs (from RFC 4122, Appendix C).
        /// </summary>
        public static readonly Guid IsoOidNamespace = new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8");

        /// <summary>
        ///     Create sequential Guid.
        /// </summary>
        /// <returns></returns>
        public static Guid NewSequentialUuid()
        {
            Guid guid;
            NativeMethods.UuidCreateSequential(out guid);
            return guid;
        }

        /// <summary>
        ///     Creates a new sequential Guid compatible with SQL Server sequential Guids
        ///     (using network byte order, MSB-first).
        ///     http://blogs.msdn.com/b/dbrowne/archive/2012/07/03/how-to-generate-sequential-guids-for-sql-server-in-net.aspx
        /// </summary>
        /// <returns></returns>
        public static Guid NewSqlSequentialId()
        {
            var newGuid = NewSequentialUuid().ToByteArray();
            SwapByteOrder(newGuid);
            return new Guid(newGuid);
        }

        /// <summary>
        ///     Creates a name-based UUID using the algorithm from RFC 4122 §4.3.
        /// </summary>
        /// <param name="namespaceId">The ID of the namespace.</param>
        /// <param name="name">The name (within that namespace).</param>
        /// <param name="version">
        ///     The version number of the UUID to create; this value must be either
        ///     3 (for MD5 hashing) or 5 (for default SHA-1 hashing).
        /// </param>
        /// <returns>A UUID derived from the namespace and name.</returns>
        /// <remarks>
        ///     See <a href="http://code.logos.com/blog/2011/04/generating_a_deterministic_guid.html">Generating a deterministic GUID</a>.
        /// </remarks>
        public static Guid NewNamespaceUuid(Guid namespaceId, string name, int version = 5)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (version != 3 && version != 5) throw new ArgumentOutOfRangeException("version", "version must be either 3 or 5.");

            // convert the name to a sequence of octets (as defined by the standard or conventions of its namespace) (step 3)
            // ASSUME: UTF-8 encoding is always appropriate
            var nameBytes = Encoding.UTF8.GetBytes(name);

            // convert the namespace UUID to network order (step 3)
            var namespaceBytes = namespaceId.ToByteArray();
            SwapByteOrder(namespaceBytes);

            // compute the hash of the name space ID concatenated with the name (step 4)
            byte[] hash;
            using (var algorithm = version == 3 ? (HashAlgorithm)MD5.Create() : SHA1.Create())
            {
                algorithm.TransformBlock(namespaceBytes, 0, namespaceBytes.Length, null, 0);
                algorithm.TransformFinalBlock(nameBytes, 0, nameBytes.Length);
                hash = algorithm.Hash;
            }

            // most bytes from the hash are copied straight to the bytes of the new GUID (steps 5-7, 9, 11-12)
            var newGuid = new byte[16];
            Array.Copy(hash, 0, newGuid, 0, 16);

            // set the four most significant bits (bits 12 through 15) of the time_hi_and_version
            // field to the appropriate 4-bit version number from Section 4.1.3 (step 8)
            newGuid[6] = (byte)((newGuid[6] & 0x0F) | (version << 4));

            // set the two most significant bits (bits 6 and 7) of the clock_seq_hi_and_reserved
            // to zero and one, respectively (step 10)
            newGuid[8] = (byte)((newGuid[8] & 0x3F) | 0x80);

            // convert the resulting UUID to local byte order (step 13)
            SwapByteOrder(newGuid);
            return new Guid(newGuid);
        }

        // Converts a GUID (expressed as a byte array) to/from network order (MSB-first).
        private static void SwapByteOrder(byte[] guid)
        {
            SwapBytes(guid, 0, 3);
            SwapBytes(guid, 1, 2);
            SwapBytes(guid, 4, 5);
            SwapBytes(guid, 6, 7);
        }

        private static void SwapBytes(byte[] guid, int left, int right)
        {
            var temp = guid[left];
            guid[left] = guid[right];
            guid[right] = temp;
        }

        private static class NativeMethods
        {
            [DllImport("rpcrt4.dll", SetLastError = true)]
            public static extern int UuidCreateSequential(out Guid guid);
        }
    }

}