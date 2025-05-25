//Copyright (c) 2007-2008 Henrik Schröder, Oliver Kofoed Pedersen

using System.Security.Cryptography;

//Permission is hereby granted, free of charge, to any person
//obtaining a copy of this software and associated documentation
//files (the "Software"), to deal in the Software without
//restriction, including without limitation the rights to use,
//copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the
//Software is furnished to do so, subject to the following
//conditions:

//The above copyright notice and this permission notice shall be
//included in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
//OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
//NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
//HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
//WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

namespace Cezzi.Caching.Memcached
{
    /// <summary>
    /// Fowler-Noll-Vo hash, variant 1, 32-bit version.
    /// http://www.isthe.com/chongo/tech/comp/fnv/
    /// </summary>
    public class FNV1_32 : HashAlgorithm
    {
        private static readonly uint FNV_prime = 16777619;
        private static readonly uint offset_basis = 2166136261;

        /// <summary>The hash</summary>
        protected uint hash;

        /// <summary>Initializes a new instance of the <see cref="FNV1_32"/> class.</summary>
        public FNV1_32()
        {
            HashSizeValue = 32;
        }

        /// <summary>
        /// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm"></see> class.
        /// </summary>
        public override void Initialize()
        {
            hash = offset_basis;
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int length = ibStart + cbSize;
            for (int i = ibStart; i < length; i++)
            {
                hash = (hash * FNV_prime) ^ array[i];
            }
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(hash);
        }
    }

    /// <summary>
    /// Fowler-Noll-Vo hash, variant 1a, 32-bit version.
    /// http://www.isthe.com/chongo/tech/comp/fnv/
    /// </summary>
    public class FNV1a_32 : HashAlgorithm
    {
        private static readonly uint FNV_prime = 16777619;
        private static readonly uint offset_basis = 2166136261;

        /// <summary>The hash</summary>
        protected uint hash;

        /// <summary>Initializes a new instance of the <see cref="FNV1a_32"/> class.</summary>
        public FNV1a_32()
        {
            HashSizeValue = 32;
        }

        /// <summary>
        /// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm"></see> class.
        /// </summary>
        public override void Initialize()
        {
            hash = offset_basis;
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int length = ibStart + cbSize;
            for (int i = ibStart; i < length; i++)
            {
                hash = (hash ^ array[i]) * FNV_prime;
            }
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            return BitConverter.GetBytes(hash);
        }
    }

    /// <summary>
    /// Modified Fowler-Noll-Vo hash, 32-bit version.
    /// http://home.comcast.net/~bretm/hash/6.html
    /// </summary>
    public class ModifiedFNV1_32 : FNV1_32
    {
        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>The computed hash code.</returns>
        protected override byte[] HashFinal()
        {
            hash += hash << 13;
            hash ^= hash >> 7;
            hash += hash << 3;
            hash ^= hash >> 17;
            hash += hash << 5;
            return BitConverter.GetBytes(hash);
        }
    }
}