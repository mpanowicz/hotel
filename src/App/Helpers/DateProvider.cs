namespace App.Helpers;

public interface IDateProvider
{
    DateOnly Today();
}

internal class DateProvider : IDateProvider
{
    public DateOnly Today()
    {
        return DateOnly.FromDateTime(DateTime.Now);
    }
}
