namespace DevChatter.Bot.Core.Data.Model
{
	public class CommandWordEntity : DataEntity
	{
		public string Word { get; set; }
		public string TypeFqdn { get; set; }
	}
}