using System;
using System.Runtime.CompilerServices;

namespace MusicStore.Models.Module
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class DBTableAttribute : Attribute
    {
        public string TableName { get; set; }
        public DBTableAttribute([CallerMemberName] string Name = null)
        {
            TableName = Name;
        }
    }
}