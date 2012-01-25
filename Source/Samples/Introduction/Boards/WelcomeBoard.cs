﻿using System;
using System.Collections.Generic;
using System.Linq;

using TextAdventure.Engine.Common;
using TextAdventure.Engine.Game.Commands;
using TextAdventure.Engine.Game.Events;
using TextAdventure.Engine.Game.Messages;
using TextAdventure.Engine.Objects;
using TextAdventure.Samples.Factories;
using TextAdventure.Samples.Introduction.Actors;
using TextAdventure.Samples.Introduction.SoundEffects;

namespace TextAdventure.Samples.Introduction.Boards
{
	public class WelcomeBoard : Board
	{
		public static readonly Guid BoardId = Guid.Parse("dae415ca-ca40-4745-8126-217e43530170");
		public static readonly Size BoardSize = new Size(80, 20);
		private static readonly Coordinate _layerOriginCoordinate;
		private static readonly Size _layerSize = new Size(21, 7);

		static WelcomeBoard()
		{
			_layerOriginCoordinate = new Coordinate((BoardSize.Width / 2) - (_layerSize.Width / 2), 8);
		}

		public WelcomeBoard()
			: base(BoardId, "Welcome", "", BoardSize, GetBackgroundLayer(), GetForegroundLayer(), GetActorInstanceLayer(), GetExits())
		{
		}

		private static SpriteLayer GetBackgroundLayer()
		{
			var character = new Character(Symbol.LightShade, new Color(14, 119, 11), new Color(9, 80, 8));
			IEnumerable<Sprite> sprites = SpriteFactory.Instance.CreateArea(_layerOriginCoordinate, _layerSize, character);

			return new SpriteLayer(BoardSize, sprites);
		}

		private static SpriteLayer GetForegroundLayer()
		{
			var character = new Character(Symbol.Number, Color.White, Color.TransparentBlack);
			IEnumerable<Sprite> borderSprites = SpriteFactory.Instance.CreateBorder(_layerOriginCoordinate, _layerSize, character);
			IEnumerable<Sprite> textLine1Sprites = SpriteFactory.Instance.CreateCenteredText(
				"Text Adventure Game Engine Using XNA",
				0,
				BoardSize.Width,
				Color.Yellow,
				Color.TransparentBlack);
			IEnumerable<Sprite> textLine2Sprites = SpriteFactory.Instance.CreateCenteredText(
				"Hosted by Nathan Alden, Sr. and ctxna.org",
				2,
				BoardSize.Width,
				Color.White,
				Color.TransparentBlack);
			IEnumerable<Sprite> textLine3Sprites = SpriteFactory.Instance.CreateCenteredText(
				"https://github.com/NathanAlden/TextAdventure",
				4,
				BoardSize.Width,
				Color.White,
				Color.TransparentBlack);
			IEnumerable<Sprite> textLine4Sprites = SpriteFactory.Instance.CreateCenteredText(
				"http://blog.TheCognizantCoder.com",
				6,
				BoardSize.Width,
				Color.White,
				Color.TransparentBlack);
			var sprites = new List<Sprite>(borderSprites
			                               	.Concat(textLine1Sprites)
			                               	.Concat(textLine2Sprites)
			                               	.Concat(textLine3Sprites)
			                               	.Concat(textLine4Sprites));
			var doorCoordinate = new Coordinate(_layerOriginCoordinate.X + _layerSize.Width - 1, _layerOriginCoordinate.Y + 3);

			sprites.RemoveAll(arg => arg.Coordinate == doorCoordinate);
			sprites.Add(new Sprite(doorCoordinate, new Character(Symbol.InverseCircle, Color.Magenta, Color.Black)));

			return new SpriteLayer(BoardSize, sprites);
		}

		private static ActorInstanceLayer GetActorInstanceLayer()
		{
			var welcomeActor = new WelcomeActor();

			ActorInstance actorInstance = ActorInstanceFactory.Instance.CreateActorInstance(
				welcomeActor,
				new Coordinate(BoardSize.Width / 2, 10),
				playerTouchedActorInstanceEventHandler:new PlayerTouchedWelcomeActorEventHandler());

			return new ActorInstanceLayer(BoardSize, new[] { actorInstance });
		}

		private static IEnumerable<BoardExit> GetExits()
		{
			yield return new BoardExit(new Coordinate(50, 11), BoardExitDirection.Right, Guid.NewGuid(), Coordinate.Zero);
		}

		private class PlayerTouchedWelcomeActorEventHandler : Engine.Game.Events.EventHandler<PlayerTouchedActorInstanceEvent>
		{
			public override void HandleEvent(EventContext context, PlayerTouchedActorInstanceEvent @event)
			{
				Color indent0 = Color.Yellow;
				Color indent1 = Color.White;
				MessageBuilder messageBuilder = Message
					.Build(Color.DarkBlue)
					.Text(indent0, "Introduction", 1)
					.Text(indent1, "  - Who am I?", 1)
					.Text(indent0, "What is ZZT?", 1)
					.Text(indent1, "  - Created by Tim Sweeney (of Unreal fame) in 1991", 1)
					.Text(indent1, "  - ZZT is accessible -- you don't have to be an artist or sound engineer to create a game", 1)
					.Text(indent0, "Why recreate an old DOS game?", 1)
					.Text(indent1, "  - Rekindle the awesomeness of ZZT using modern tools and graphics capabilities", 1)
					.Text(indent1, "  - For the challenge and learning experience", 1)
					.Text(indent1, "  - For fun!");

				context.EnqueueCommand(Commands.Message(messageBuilder));

				messageBuilder = Message
					.Build(Color.DarkRed)
					.Question(
						"Unlock the next board?",
						Color.Yellow,
						Color.White,
						Color.Yellow,
						Color.Gray,
						MessageAnswer.Build(Guid.Parse("233587f3-e960-4f78-bf66-c4c20110ca05"), "Yes", new YesAnswerSelectedEventHandler(@event.Target)),
						MessageAnswer.Build("No"));

				context.EnqueueCommand(Commands.Message(messageBuilder));
			}
		}

		private class YesAnswerSelectedEventHandler : Engine.Game.Events.EventHandler<MessageAnswerSelectedEvent>
		{
			private readonly ActorInstance _actorInstance;

			public YesAnswerSelectedEventHandler(ActorInstance actorInstance)
			{
				_actorInstance = actorInstance;
			}

			public override void HandleEvent(EventContext context, MessageAnswerSelectedEvent @event)
			{
				context.PlayerInput.Suspend();

				ChainedCommand moveActorCommand = Commands
					.Chain(Commands
					       	.ActorInstanceMoveRight(_actorInstance)
					       	.Repeat(TimeSpan.FromMilliseconds(100), 9))
					.And(Commands.Delay(TimeSpan.FromMilliseconds(100)))
					.And(Commands.ActorInstanceMoveDown(_actorInstance))
					.And(Commands.Delay(TimeSpan.FromMilliseconds(500)))
					.And(Commands.PlaySoundEffect(context.GetSoundEffectById(ExplodeSoundEffect.SoundEffectId), Volume.Full))
					.And(Commands.RemoveSprite(context.CurrentBoard.ForegroundLayer, new Coordinate(50, 11)))
					.And(Commands.ActorInstanceDestroy(_actorInstance))
					.And(Commands.PlayerResumeInput());

				if (context.Player.Coordinate == new Coordinate(_actorInstance.Coordinate.X + 1, _actorInstance.Coordinate.Y))
				{
					ChainedCommand command = Commands
						.Chain(Commands.Delay(TimeSpan.FromSeconds(1)))
						.And(Commands.PlaySoundEffect(context.GetSoundEffectById(SlapSoundEffect.SoundEffectId), Volume.Full))
						.And(Commands.PlayerMoveDown())
						.And(Commands.Message(Message.Build(Color.DarkRed).Text(Color.Yellow, "WHAP!")))
						.And(Commands.Delay(TimeSpan.FromSeconds(1)))
						.And(moveActorCommand);

					context.EnqueueCommand(command);
				}
				else
				{
					context.EnqueueCommand(moveActorCommand);
				}
			}
		}
	}
}