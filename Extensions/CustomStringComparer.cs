namespace NewSky.API.Extensions
{
    public class CustomStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (char.IsLetter(x[0]) && !char.IsLetter(y[0]))
                return -1; 
            else if (!char.IsLetter(x[0]) && char.IsLetter(y[0]))
                return 1; 
            else if (char.IsDigit(x[0]) && !char.IsDigit(y[0]))
                return -1; 
            else if (!char.IsDigit(x[0]) && char.IsDigit(y[0]))
                return 1; 
            else
                return x.CompareTo(y);
        }
    }

}
