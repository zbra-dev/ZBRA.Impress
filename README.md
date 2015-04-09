# ZBRA Impress
Useful classes for .NET everyday projects.

# Description
ZBRA Impress is a C# written library that aims to improve the programmer's experience when coding .NET applications.
The goal is to make it a simpler, more fluent and fun experience. This is done by means of removing bloilerplate code by introducing more powerful asbtractions.
As a simple example you can do this :

	var dictionary = new Dictionary<int, int> (); // loaded with month to number of days in month
 
	 public boolean IsMonthLonger(int keyMonth) 
	 {
		 int dayInMonth; 
		 if (dictionary.TryGet(keyMonth , out dayInMonth){
			 // key if found
			 return dayInMonth == 31;
		 } else {
			 throw new Exception("Value not found in dictionary");
		 }
	}



Or you can use Maybe and write


	 var dictionary = new Dictionary<int, int> (); // loaded with month to number of days in month
	 
	 public boolean IsMonthLonger(int keyMonth) 
	 {
		 return dictionary.MaybeGet(keyMonth).Is(31).Value;
	 }


It will return the correct boolean value if present, or throw an exception if the key is not present.
The complexy is reduced in unexpected and impressive ways when you use the Maybe monad implementation.

Similar impressive results arise from usign other class types in the ZBRA Impress API and for this reason we are sharing them with you.

# ZBRA Impress Core

ZBRA Impress is itself a set of assemblies you can reference in you projects to gain access to out-of-the-box useful classes for everyday programming tasks.

The core assembly introduces extentions to the .NET core language, types, and API features. The main feature is the Maybe class type that implements 
the [Maybe](http://zbra.com.br/2013/11/13/monads-em-csharp/) [Monad](http://en.wikipedia.org/wiki/Monad_%28functional_programming%29)
that acts like the Nullable class type but allows for every kind of type and not only structs. Introducing Maybe in the .NET ecosystem rapidly implies in several extention methods
to existing classe types, specially, but not limited to, the Collections API. Maybe fits nicelly with IEnumerable and the platform support for monands and Linq.

You will also find an implementation of the Rational Design Pattern in the form of the ```Fraction``` class type. ```Fraction``` allows arbitrary precision representation 
of rational numbers, together with arbitrary precision calculations over those rational numbers.

Other features include a full fledged ```Interval``` classe type that leverages the Maybe concept, an ```Hash``` class type to help you calculate the return 
of ```GetHashCode``` in more complex scenarios, and some utilitary extentions for ```string``` and ```enum``` types.

# ZBRA Impress Extra Collections

This is an assemby dedicated to new types of collections that are not available in the standard .NET API. These include the bidirectionay dictionary (for mapping keys to values and values back to their keys),
and IMultiSet interface for [bags](http://en.wikipedia.org/wiki/Multiset), a IMltiMap interface for dictionary like collections that map keys to collections of values and a simple interface for a Concurrent Dictionary.

All these new extra collections have a readonly version and a separate interface for each of the readonly versions. This allows for better classification and empowers better encapsulation by 
strictly adhere to the [Interface Segregation Principle](http://en.wikipedia.org/wiki/Interface_segregation_principle).

# ZBRA Impress Globalization

An alternative API to work with Globalization namely with Localization of captions, labels and system messages. The API can use the resx files to acomodate translations, but is not limited to that strategy.
Also some classes are present to handle cultural preferences by means of an ```ICultureSelector```.

Translation strategies can be composed by means of ```IMessageProcessor```.

#ZBRA Impress Validation

Provides an extensible validation framework to use in any layer of you application. The frameworks leverages Generic types and reification to allow composition of validators. 
Attributes can be used for simple POCO validation with ```AnnotatedPropertyBagValidator``` but you can opt-in for this strategy as it is not mandatory.