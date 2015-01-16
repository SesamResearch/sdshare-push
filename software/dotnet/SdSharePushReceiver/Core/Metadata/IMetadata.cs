using System;
using System.Collections.Generic;
using NetTriple;

namespace SdShare.Metadata
{
    public interface IMetadata
    {
        DateTime TimeUtc { get; }
        IEnumerable<Triple> Triples { get; } 
    }
}
