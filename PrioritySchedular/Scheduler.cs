public class Scheduler
{
    private List<Event> events;
    private Dictionary<string, Dictionary<string, int>> transitionMatrix;

    public Scheduler(List<Event> events, List<LocationTransition> transitions)
    {
        this.events = events;
        this.transitionMatrix = new Dictionary<string, Dictionary<string, int>>();

        foreach (var transition in transitions)
        {
            if (!transitionMatrix.ContainsKey(transition.From))
                transitionMatrix[transition.From] = new Dictionary<string, int>();
            transitionMatrix[transition.From][transition.To] = transition.DurationMinutes;

            if (!transitionMatrix.ContainsKey(transition.To))
                transitionMatrix[transition.To] = new Dictionary<string, int>();
            transitionMatrix[transition.To][transition.From] = transition.DurationMinutes;
        }
    }

    public (int maxEvents, List<int> eventIds, int totalValue) ScheduleEvents()
    {
        List<Event> scheduledEvents = new List<Event>();
        TimeSpan currentTime = TimeSpan.Zero;
        string currentLocation = "";

        var sortedEvents = events.OrderBy(e => e.StartTime).ThenByDescending(e => e.Priority).ToList();

        for (int i = 0; i < sortedEvents.Count; i++)
        {
            var evt = sortedEvents[i];
           
            Event lastSecondEvent = null;
            Event lastEvent = null;

            if(scheduledEvents.Count >=2 )
            {
                lastSecondEvent = scheduledEvents[scheduledEvents.Count - 2];
                lastEvent = scheduledEvents[scheduledEvents.Count - 1];
            }

            if (currentTime <= evt.StartTime)
            {
                scheduledEvents.Add(evt);
                currentTime = evt.EndTime;
                currentLocation = evt.Location;
            }else if (lastEvent != null && CanTravel(lastSecondEvent.Location, evt.Location, lastSecondEvent.EndTime, evt.StartTime) )
            {
                if(lastEvent.Priority < evt.Priority)
                {
                    scheduledEvents.Remove(lastEvent);
                    scheduledEvents.Add(evt);
                    currentTime = evt.EndTime;
                    currentLocation = evt.Location;
                }
            }
            else if (currentLocation != evt.Location && CanTravel(currentLocation, evt.Location, currentTime, evt.StartTime))
            {
                currentTime += TimeSpan.FromMinutes(GetTravelTime(currentLocation, evt.Location));
                if (currentTime <= evt.StartTime)
                {
                    scheduledEvents.Add(evt);
                    currentTime = evt.EndTime;
                    currentLocation = evt.Location;
                }
            }
        }

        int totalValue = scheduledEvents.Sum(e => e.Priority);
        return (scheduledEvents.Count, scheduledEvents.Select(e => e.Id).ToList(), totalValue);
    }

    private bool CanTravel(string from, string to, TimeSpan startTime, TimeSpan endTime)
    {
        if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) ||
            !transitionMatrix.ContainsKey(from) || !transitionMatrix[from].ContainsKey(to))
            return false;

        int travelDuration = GetTravelTime(from, to);
        return (startTime - TimeSpan.FromMinutes(travelDuration)) >= TimeSpan.Zero;
    }

    private int GetTravelTime(string from, string to)
    {
        return transitionMatrix[from][to];
    }
}