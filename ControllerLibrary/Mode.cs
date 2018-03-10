namespace ReceiverController
{
	public class Mode
	{
		private readonly string _flag;
		private readonly string _name;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mode"/> class.
		/// </summary>
		/// <param name="name">Name of the mode.</param>
		/// <param name="flag">The mode flag.</param>
		public Mode(string name, string flag)
		{
			this._name = name;
			this._flag = flag;
		}

		/// <summary>
		/// Gets the mode flag.
		/// </summary>
		/// <value>The mode flag.</value>
		public string Flag
		{
			get
			{
				return this._flag;
			}
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return this._name;
			}
		}
	}
}