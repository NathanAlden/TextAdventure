using System;
using System.Collections.Generic;

using Junior.Common;

using TextAdventure.Engine.Game.Events;

namespace TextAdventure.Engine.Objects
{
	public class World : IUnique
	{
		private readonly Dictionary<Guid, Actor> _actorsById = new Dictionary<Guid, Actor>();
		private readonly IEventHandler<AnswerSelectedEvent> _answerSelectedEventHandler;
		private readonly Dictionary<Guid, Board> _boardsById = new Dictionary<Guid, Board>();
		private readonly Guid _id;
		private readonly Dictionary<Guid, Message> _messagesById = new Dictionary<Guid, Message>();
		private readonly Dictionary<Guid, Song> _songsById = new Dictionary<Guid, Song>();
		private readonly Dictionary<Guid, SoundEffect> _soundEffectsById = new Dictionary<Guid, SoundEffect>();
		private readonly Player _startingPlayer;
		private readonly Dictionary<Guid, Timer> _timersById = new Dictionary<Guid, Timer>();
		private readonly string _title;
		private readonly int _version;

		public World(
			Guid id,
			int version,
			string title,
			Player startingPlayer,
			IEnumerable<Board> boards,
			IEnumerable<Actor> actors,
			IEnumerable<Message> messages,
			IEnumerable<Timer> timers,
			IEnumerable<SoundEffect> soundEffects,
			IEnumerable<Song> songs,
			IEventHandler<AnswerSelectedEvent> answerSelectedEventHandler = null)
		{
			title.ThrowIfNull("title");
			startingPlayer.ThrowIfNull("startingPlayer");
			boards.ThrowIfNull("boards");
			actors.ThrowIfNull("actors");
			messages.ThrowIfNull("messages");
			timers.ThrowIfNull("timers");
			soundEffects.ThrowIfNull("soundEffects");
			songs.ThrowIfNull("songs");
			if (version < 1)
			{
				throw new ArgumentOutOfRangeException("version", "Version must be at least 1.");
			}

			_id = id;
			_version = version;
			_title = title;
			_answerSelectedEventHandler = answerSelectedEventHandler;
			_startingPlayer = startingPlayer;
			foreach (Board board in boards)
			{
				_boardsById.Add(board.Id, board);
			}
			foreach (Actor actor in actors)
			{
				_actorsById.Add(actor.Id, actor);
			}
			foreach (Message message in messages)
			{
				_messagesById.Add(message.Id, message);
			}
			foreach (Timer timer in timers)
			{
				_timersById.Add(timer.Id, timer);
			}
			foreach (SoundEffect soundEffect in soundEffects)
			{
				_soundEffectsById.Add(soundEffect.Id, soundEffect);
			}
			foreach (Song song in songs)
			{
				_songsById.Add(song.Id, song);
			}
		}

		public int Version
		{
			get
			{
				return _version;
			}
		}

		public string Title
		{
			get
			{
				return _title;
			}
		}

		public Player StartingPlayer
		{
			get
			{
				return _startingPlayer;
			}
		}

		public IEnumerable<Board> Boards
		{
			get
			{
				return _boardsById.Values;
			}
		}

		public IEnumerable<Actor> Actors
		{
			get
			{
				return _actorsById.Values;
			}
		}

		public IEnumerable<Message> Messages
		{
			get
			{
				return _messagesById.Values;
			}
		}

		public IEnumerable<Timer> Timers
		{
			get
			{
				return _timersById.Values;
			}
		}

		public IEnumerable<SoundEffect> SoundEffects
		{
			get
			{
				return _soundEffectsById.Values;
			}
		}

		public IEnumerable<Song> Songs
		{
			get
			{
				return _songsById.Values;
			}
		}

		public IEventHandler<AnswerSelectedEvent> AnswerSelectedEventHandler
		{
			get
			{
				return _answerSelectedEventHandler;
			}
		}

		public Guid Id
		{
			get
			{
				return _id;
			}
		}

		public Board GetBoardById(Guid id)
		{
			return _boardsById[id];
		}

		public Actor GetActorById(Guid id)
		{
			return _actorsById[id];
		}

		public Message GetMessageById(Guid id)
		{
			return _messagesById[id];
		}

		public Timer GetTimerById(Guid id)
		{
			return _timersById[id];
		}

		public SoundEffect GetSoundEffectById(Guid id)
		{
			return _soundEffectsById[id];
		}

		public Song GetSongById(Guid id)
		{
			return _songsById[id];
		}
	}
}