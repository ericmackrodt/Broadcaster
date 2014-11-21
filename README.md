Broadcaster
===========

A simple messaging .Net library to send messages between objects (ViewModels for example).

Usage
-----

You should use this library with a Dependency Injection Container, such as Autofac.

In order to use the library you have to create an event, it has to inherit from BroadcasterEvent and you have to have a class to be the Event content.

```
public class DummyMessage
{
	public string Content { get; set; }
}

public class DummyEvent : BroadcasterEvent<DummyMessage> { }
```

Now, you have to subscribe to the event, you can subscribe multiple times and in multiple locations to the same event.

```
public class SubscribingViewModel
{
	private IBroadcaster _broadcaster; // It has to be an instance of BroadcastContainer
	
	public SubscribingViewModel(IBroadcaster broadcaster)
	{
		_broadcaster = broadcaster;
		
		_broadcaster.Event<DummyEvent>().Subscribe(DummyMethod);
	}
	
	private void DummyMethod(DummyMessage message) 
	{
	}
}
```

Then you broadcast the event from another ViewModel or place.

```
public class BroadcastingViewModel
{
	private IBroadcaster _broadcaster; // It has to be an instance of BroadcastContainer
	
	private ICommand _buttonCommand;
	//This is a command that is fired from the interface.
	public ICommand ButtonCommand
	{
		get { return _buttonCommand; }
	}
	
	public BroadcastingViewModel(IBroadcaster broadcaster)
	{
		_broadcaster = broadcaster;
		
		_buttonCommand = new RelayCommand(Command);
	}
	
	private void Command(object obj)
	{
		//We are broadcasting from a command, but you could broadcast from anywhere
		_broadcaster.Event<DummyEvent>().Broadcast(new DummyMessage() 
		{
			Content = "Something"
		});
	}
}
```