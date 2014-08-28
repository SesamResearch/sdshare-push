namespace SdShare
{
    public interface IFragmentReceiver
    {
        void Receive(string resourceUri, string graphUri, string payload);
    }
}
