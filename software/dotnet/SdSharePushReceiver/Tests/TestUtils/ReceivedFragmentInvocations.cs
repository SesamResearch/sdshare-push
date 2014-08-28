namespace TestUtils
{
    public static class ReceivedFragmentInvocations
    {
        public static int InvocationCountTypeA { get; private set; }
        public static int InvocationCountTypeB { get; private set; }

        public static void Reset()
        {
            InvocationCountTypeA = 0;
            InvocationCountTypeB = 0;
        }

        public static void IncTypeA()
        {
            InvocationCountTypeA = InvocationCountTypeA + 1;
        }

        public static void IncTypeB()
        {
            InvocationCountTypeB = InvocationCountTypeB + 1;
        }
    }
}
