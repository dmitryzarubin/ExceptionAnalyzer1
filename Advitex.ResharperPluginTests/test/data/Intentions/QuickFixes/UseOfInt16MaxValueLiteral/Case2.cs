using System;

  public class MyException : Exception
  {
	public MyException()
	{
	}
  }
  
class C
{


  ///<summary>
  /// MySummary 11
  /// MySummary 22
  ///</summary>
  /// <param name = "throwStatement"> throw выражение asfadsfdf </param>  
  /// <exception cref="ArgumentNullException">My exception description</exception>
  public void Main()
  {
    var ex = new ArgumentException("Not assigned 3");
  
	throw new ArgumentException1("Unresolved exception");  
	throw new MyException("Unresolved exception");
	
	try
	{
		throw new {caret}ArgumentException("Not assigned 1");
	}
	catch(InvalidOperationException)
	{
	}
	
	try
	{
		throw new System.ArgumentException("Not assigned 2");
	}
	catch(ArgumentException)
	{
	}
	
	try
	{
		throw new System.ArgumentNullException("Not assigned 3");
	}
	catch(ArgumentException)
	{
	}
	
	try
	{
		throw new System.ArgumentNullException("Not assigned 4");
	}
	catch(Exception)
	{
	}
	
	try
	{
		throw new System.ArgumentNullException("Not assigned 5");
	}
	catch
	{
	}

	
	try
	{
		try
		{
			throw new System.ArgumentNullException("Not assigned 5");
		}
		catch(InvalidOperationException)
		{
		}
	}
	catch(ArgumentException)
	{
	}
	
	throw ex;
	throw GetEx();
  
    throw ArgumentException("Not assigned 3");
	throw 
  
 
    (9223372036854775807L - 1).ToString();
  }
  
  public Exception GetEx()
  {
  return  new ArgumentException("Not assigned 4"); 
  }
  
}