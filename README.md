Simple SQL-based Cache for .NET
===============================

(Forked from https://github.com/ovicus/sqlcache)

.NET 4.0 introduced a new caching API that extends the previous existing API 
intended only for ASP.NET applications. The core of this new caching mechanism is the abstract class ObjectCache 
in the namespace System.Runtime.Caching. .NET provides a concrete implementation for in-memory caching 
through the MemoryCache class. However the in-memory cache is not suitable for some distributed scenarios. 

This project provides a concrete implementation of the .NET Caching API, based on SQL Server, 
suitable for distributed scenarios.

The SqlCache implementation requires a table that should be created through the provided SQL script.

A Flush method is provided that can be called from some kind of scheduled task (ex: AzureWebjobs).
Another better approach to remove expired entries is using a SQL Job.

Changes from my fork (compared to https://github.com/ovicus/sqlcache)
--------------------------
- I use JsonNet for serialization.
- My flush method deletes sliding expiration items
- Fixed bug when setting values with sliding expiration (original code ignores sliding expiration due a bug when comparing DateTimeOffset)

No nuget package yet. It's a single file, you can copy&paste SqlCache.cs, and install-package newtonsoft.json and your'e good to go.

TODOs:
- GetCount() does not ignore expired items with slidingexpiration 
- Shound be able to pass JsonSerializerSettings on the constructor

Another comments:

Although a ChangeMonitor could be implemented, I like ovicus idea that you have to manually flush the expired items.
If you implement a ChangeMonitor, it will be constantly "touching" the database in order to check for expired items and that could be an issue.