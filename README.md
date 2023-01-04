![NuGet](https://img.shields.io/nuget/v/SquidEyes.Futures)
![Downloads](https://img.shields.io/nuget/dt/squideyes.futures)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

**SquidEyes.Futures** is a small set of trading primitives, helper classes, extension methods and trading utilities; **most notably**:

* **TickSet**: A high-performance, low-memory, tick-data collection that manages data on a Source+Contract+TradeDate basis
* **Known**: A static helper class that defines the Assets, Trade-Dates, Contracts and SymbolAs-conversions that SquidEyes.Trading works with
* **WickoFeed**: A helper-class that converts ticks in Renko-style candles, but with High/Low wicks
* **TdRenkoFeed**: A helper-class that converts ticks in TdRenko-style candles
* **TickSetMaker**: Converts and normalizes continuous Kibot futures data to one or more TickSets (see <a href="http://www.kibot.com/buy.aspx#futures" target="_blank">http://www.kibot.com/buy.aspx#futures</a>)

The software is open-sourced under a standard MIT license (see **LICENSE.md** for further details).  

Contributions are always welcome (see **CONTRIBUTING.md** for further details).

The code is mostly for the author's own personal use so there is no documentation on offer, aside from this README.md, nor is there any intent (on the author's part!) to create more-detailed documention, going forward.  

As to unit-tests, a decent percent of the code is covered, but the tests should not be looked upon as being comprehensive.

**Caveat Emptor**:  THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.



