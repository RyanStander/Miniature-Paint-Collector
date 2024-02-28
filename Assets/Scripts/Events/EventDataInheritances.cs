//Event that informs subscribers of a debug log

using Crew;
using TimeAndSeasons;
using Weather;

namespace Events
{
    public class DateChange : EventData
    {
        public readonly int CurrentDay;
        public readonly int CurrentMonth;
        public readonly Season CurrentSeason;
        public readonly int CurrentYear;

        public DateChange(int currentDay, int currentMonth, Season currentSeason, int currentYear) : base(
            EventIdentifiers.DateChange)
        {
            CurrentDay = currentDay;
            CurrentMonth = currentMonth;
            CurrentSeason = currentSeason;
            CurrentYear = currentYear;
        }
    }

    public class SendTimeStrength : EventData
    {
        public readonly float TimeStrength;
        public SendTimeStrength(float timeStrength) : base(EventIdentifiers.SendTimeStrength)
        {
            TimeStrength = timeStrength;
        }
    }

    public class NewDay : EventData
    {
        public NewDay():base(EventIdentifiers.NewDay)
        {

        }
    }

    public class WeatherHasChanged : EventData
    {
        public readonly WeatherPreset WeatherPreset;
        public WeatherHasChanged(WeatherPreset weatherPreset) : base(EventIdentifiers.WeatherHasChanged)
        {
            WeatherPreset = weatherPreset;
        }
    }

    public class NewWind : EventData
    {
        public readonly float WindSpeed;
        public readonly float WindDirection;
        public NewWind(float windSpeed, float windDirection) : base(EventIdentifiers.NewWind)
        {
            WindSpeed = windSpeed;
            WindDirection = windDirection;
        }
    }

    #region Crew

    public class RecruitCrewMember : EventData
    {
        public readonly CrewMemberStats CrewMemberStats;
        public RecruitCrewMember(CrewMemberStats crewMemberStats) : base(EventIdentifiers.RecruitCrewMember)
        {
            CrewMemberStats = crewMemberStats;
        }
    }

    public class SortCrewMember : EventData
    {
        public readonly CrewMemberStats CrewMemberStats;
        public SortCrewMember(CrewMemberStats crewMemberStats) : base(EventIdentifiers.SortCrewMember)
        {
            CrewMemberStats = crewMemberStats;
        }
    }

    public class RecalculatePlayerCrewModifiers : EventData
    {
        public RecalculatePlayerCrewModifiers() : base(EventIdentifiers.RecalculatePlayerCrewModifiers)
        {
        }
    }

    #endregion


}
