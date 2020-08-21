namespace Models
{
    public class SerialCommand
    {
        public string Lasers(int red, int green, int blue)
        {
            return $"(rgb,{red}:{green}|{blue})";
        }

        public string Galvo(int x, int y)
        {
            return $"(g,{x}:{y})";
        }
    }
}