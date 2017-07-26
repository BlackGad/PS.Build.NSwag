using System;
using System.ComponentModel;

namespace PS.Build.NSwag.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    [Designer("PS.Build.Adaptation")]
    public sealed class PartialClientAttribute : Attribute
    {
       
    }
}