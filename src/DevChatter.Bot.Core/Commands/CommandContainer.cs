using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DevChatter.Bot.Core.Commands
{
	public interface ICommandResolver
	{
		Type CommandFor(string word);
		void AddCommandResolution(string word, Type type);
		string[] CommandWords { get; }
	}

	public class CommandResolver : ICommandResolver
	{
		private readonly IDictionary<string, Type> _commandWords = new Dictionary<string, Type>();

		public Type CommandFor(string word)
		{
			word = word.ToLower(CultureInfo.CurrentCulture);

			return _commandWords.ContainsKey(word) ? _commandWords[word] : null;
		}

		public void AddCommandResolution(Type type)
		{
			var typeName = type.Name.Split(new [] {"Command"}, StringSplitOptions.None)[0];
			AddCommandResolution(typeName, type);
		}

		public void AddCommandResolution(string word, Type type)
		{
			word = word.ToLower(CultureInfo.CurrentCulture);

			if (_commandWords.ContainsKey(word))
			{
				return;
			}

			_commandWords.Add(word, type);
		}

		public string[] CommandWords => _commandWords.Keys.ToArray();
	}

	public class CommandContainer : IList<IBotCommand>
	{
		private readonly IList<IBotCommand> _list;

		public CommandContainer()
		{
			_list = new List<IBotCommand>();
		}

		public EventHandler<CommandConfigurationEventArgs> CommandAdded;

		public IEnumerator<IBotCommand> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) _list).GetEnumerator();
		}

		public void Add(IBotCommand item)
		{
			CommandAdded?.Invoke(this, new CommandConfigurationEventArgs
			{
				CommandType = item.GetType()
			});

			_list.Add(item);
		}

		public void AddRange(IEnumerable<IBotCommand> items)
		{
			foreach (var item in items)
			{
				Add(item);
			}
		}

		public void Clear()
		{
			_list.Clear();
		}

		public bool Contains(IBotCommand item)
		{
			return _list.Contains(item);
		}

		public void CopyTo(IBotCommand[] array, int arrayIndex)
		{
			_list.CopyTo(array, arrayIndex);
		}

		public bool Remove(IBotCommand item)
		{
			return _list.Remove(item);
		}

		public int Count => _list.Count;

		public bool IsReadOnly => _list.IsReadOnly;

		public int IndexOf(IBotCommand item)
		{
			return _list.IndexOf(item);
		}

		public void Insert(int index, IBotCommand item)
		{
			_list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
		}

		public IBotCommand this[int index]
		{
			get => _list[index];
			set => _list[index] = value;
		}
	}
}