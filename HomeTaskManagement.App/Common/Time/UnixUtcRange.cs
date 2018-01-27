namespace HomeTaskManagement.App.Common
{
    public struct UnixUtcRange
    {
        public UnixUtcTime Start { get; }
        public UnixUtcTime End { get; }

        public UnixUtcRange(UnixUtcTime start, UnixUtcTime end)
        {
            Start = start;
            End = end;
        }
    }
}
