﻿namespace OpenCensus.Tags
{
    using System;
    using OpenCensus.Utils;

    public sealed class TagKey : ITagKey
    {
        public const int MAX_LENGTH = 255;

        public string Name { get; }

        internal TagKey(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        public static ITagKey Create(string name)
        {
            if (!IsValid(name))
            {
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            return new TagKey(name);
        }

        public override string ToString()
        {
            return "TagKey{"
                + "name=" + Name
                + "}";
        }

        public override bool Equals(object o)
        {
            if (o == this)
            {
                return true;
            }

            if (o is TagKey)
            {
                TagKey that = (TagKey)o;
                return this.Name.Equals(that.Name);
            }

            return false;
        }

        public override int GetHashCode()
        {
            int h = 1;
            h *= 1000003;
            h ^= this.Name.GetHashCode();
            return h;
        }

        private static bool IsValid(string value)
        {
            return !string.IsNullOrEmpty(value) && value.Length <= MAX_LENGTH && StringUtil.IsPrintableString(value);
        }
    }
}
