namespace GameMaker.LocalTime.Runtime
{
    public interface ILocalTimeZoneReceiver
    {
        public void OnAddZone(LocalTimeZone zone);
        public void OnRemoveZone(LocalTimeZone zone);
    }
}