Broadcaster
===========

A simple messaging .Net library for communicating between objects (ViewModels for example).
It works like an Event Aggregator.

Usage
-----

You should use this library with a Dependency Injection Container, such as Autofac.

In order to use the library, you have to create a message, it can be any class.

```
public class DummyMessage
{
	public string Content { get; set; }
}
```

Now, you have to subscribe to the message, you can subscribe multiple times and in multiple locations to the same message.

```
public class SubscribingViewModel
{
	private IBroadcaster _broadcaster;
	
	public SubscribingViewModel(IBroadcaster broadcaster)
	{
		_broadcaster = broadcaster;
		
		_broadcaster.Subscribe<DummyMessage>(DummyMethod);
	}
	
	private void DummyMethod(DummyMessage message) 
	{
	}
}
```

Then you publish the message from another ViewModel or place.

```
public class BroadcastingViewModel
{
	private IBroadcaster _broadcaster;
	
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
		//We are publising from a command, but you could publish from anywhere
		_broadcaster.Publish(new DummyMessage() 
		{
			Content = "Something"
		});
	}
}
```