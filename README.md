# MyCalendar

Test project for MariaDb database. 

## License

Project is set to BSD-2 license but I really don't care what you do with the code. You can copy whole or parts of it and use it any way you want, or not use it at all!

## Motivation

Primrary motivation was to test MariaDb as persistent storage.

Second, project tests different approaches to solve different kind of challenges, for instance using the Result class instead of returning just a return value or throw an exception in case of error, or return tuples and so on. Some of the approaches work fine in one enviroment while not in other.

Third, to test how MariaDb can be used in tests, possibly running database in its own container during tests.

## Status

Really at initial state right now. I had to hassle a bit with getting the code into GitHub. Console application seems to work but lacks tests.

## Todos

Some of the features I've already designed but not been able to implement because of time issues.

- [ ] Web API
- [ ] Web UI
- [ ] User id & auth
- [ ] Multi user (using for instance IUSerProvider interface passed on by DI)
- [ ] Publis calendars (user rights)
- [ ] Shared calendars (user rights)
- [ ] Test cases (unit tests, integration tests, e2e tests, performance tests)
- [ ] Documentation (including this readme)
