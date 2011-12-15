﻿using System;

using TextAdventure.Engine.Common;
using TextAdventure.Engine.Game.Events;
using TextAdventure.Engine.Objects;

namespace TextAdventure.Engine.Game.Commands
{
	public static class Commands
	{
		public static ActorInstanceCreateCommand ActorInstanceCreate(
			Actor actor,
			Guid actorInstanceId,
			Coordinate coordinate,
			Character character,
			IEventHandler<ActorInstanceCreatedEvent> actorInstanceCreatedEventHandler = null,
			IEventHandler<ActorInstanceDestroyedEvent> actorInstanceDestroyedEventHandler = null,
			IEventHandler<ActorInstanceTouchedActorInstanceEvent> actorInstanceTouchedActorInstanceEventHandler = null,
			IEventHandler<PlayerTouchedActorInstanceEvent> playerTouchedActorInstanceEventHandler = null,
			IEventHandler<ActorInstanceMovedEvent> actorInstanceMovedEventHandler = null)
		{
			return new ActorInstanceCreateCommand(
				actor,
				actorInstanceId,
				coordinate,
				character,
				actorInstanceCreatedEventHandler,
				actorInstanceDestroyedEventHandler,
				actorInstanceTouchedActorInstanceEventHandler,
				playerTouchedActorInstanceEventHandler,
				actorInstanceMovedEventHandler);
		}

		public static BoardChangeCommand BoardChange(Board board, Coordinate newPlayerCoordinate)
		{
			return new BoardChangeCommand(board, newPlayerCoordinate);
		}

		public static ActorInstanceDestroyCommand ActorInstanceDestroy(ActorInstance actorInstance)
		{
			return new ActorInstanceDestroyCommand(actorInstance);
		}

		public static ActorInstanceMoveDownCommand ActorInstanceMoveDown(ActorInstance actorInstance)
		{
			return new ActorInstanceMoveDownCommand(actorInstance);
		}

		public static ActorInstanceMoveUpCommand ActorInstanceMoveUp(ActorInstance actorInstance)
		{
			return new ActorInstanceMoveUpCommand(actorInstance);
		}

		public static ActorInstanceMoveLeftCommand ActorInstanceMoveLeft(ActorInstance actorInstance)
		{
			return new ActorInstanceMoveLeftCommand(actorInstance);
		}

		public static ActorInstanceMoveRightCommand ActorInstanceMoveRight(ActorInstance actorInstance)
		{
			return new ActorInstanceMoveRightCommand(actorInstance);
		}

		public static ActorInstanceRandomMoveCommand ActorInstanceRandomMove(ActorInstance actorInstance, RandomMoveDirection directions = RandomMoveDirection.All)
		{
			return new ActorInstanceRandomMoveCommand(actorInstance, directions);
		}

		public static ActorInstanceTransportCommand ActorInstanceTransport(ActorInstance actorInstance, Coordinate coordinate)
		{
			return new ActorInstanceTransportCommand(actorInstance, coordinate);
		}

		public static PlayerMoveDownCommand PlayerMoveDown()
		{
			return new PlayerMoveDownCommand();
		}

		public static PlayerMoveUpCommand PlayerMoveUp()
		{
			return new PlayerMoveUpCommand();
		}

		public static PlayerMoveLeftCommand PlayerMoveLeft()
		{
			return new PlayerMoveLeftCommand();
		}

		public static PlayerMoveRightCommand PlayerMoveRight()
		{
			return new PlayerMoveRightCommand();
		}

		public static PlayerTransportCommand PlayerTransport(Coordinate coordinate)
		{
			return new PlayerTransportCommand(coordinate);
		}

		public static PlayerSuspendInputCommand PlayerSuspendInput()
		{
			return new PlayerSuspendInputCommand();
		}

		public static PlayerResumeInputCommand PlayerResumeInput()
		{
			return new PlayerResumeInputCommand();
		}

		public static LogCommand Log(string message)
		{
			return new LogCommand(message);
		}

		public static DelayCommand Delay(TimeSpan delay)
		{
			return new DelayCommand(delay);
		}

		public static RandomDelayCommand RandomDelay(TimeSpan minimumDelay, TimeSpan maximumDelay)
		{
			return new RandomDelayCommand(minimumDelay, maximumDelay);
		}

		public static ChainedCommand Chain(Command command)
		{
			return new ChainedCommand(command);
		}

		public static RetryCommand Retry(Command command, int maximumAttempts = MaximumAttempts.Infinite)
		{
			return new RetryCommand(command, maximumAttempts);
		}

		public static RetryCommand Retry(Command command, double retryDelayInSeconds, int maximumAttempts = MaximumAttempts.Infinite)
		{
			return new RetryCommand(command, retryDelayInSeconds, maximumAttempts);
		}

		public static RetryCommand Retry(Command command, TimeSpan retryDelay, int maximumAttempts = MaximumAttempts.Infinite)
		{
			return new RetryCommand(command, retryDelay, maximumAttempts);
		}

		public static RepeatCommand Repeat(Command command, int totalRepeats = TotalRepeats.Infinite)
		{
			return new RepeatCommand(command, totalRepeats);
		}

		public static RepeatCommand Repeat(Command command, double repeatDelayInSeconds, int totalRepeats = TotalRepeats.Infinite)
		{
			return new RepeatCommand(command, repeatDelayInSeconds, totalRepeats);
		}

		public static RepeatCommand Repeat(Command command, TimeSpan repeatDelay, int totalRepeats = TotalRepeats.Infinite)
		{
			return new RepeatCommand(command, repeatDelay, totalRepeats);
		}

		public static Command InContext(Command command, IUnique context)
		{
			return command.WithContext(context);
		}
	}
}