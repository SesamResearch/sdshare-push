using System.Collections.Generic;

namespace SdShare
{
    public interface IFragmentReceiver
    {
        void Receive(IEnumerable<string> resources, string graphUri, string payload);

        IEnumerable<string> Labels { get; set; } 
    }
}
