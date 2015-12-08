# Orchard User Notifications Readme



This Orchard module adds the ability to send simple notifications to users and groups of users.


## Documentation

The module is compatible with Orchard 1.7.x (default branch) and also with Orchard 1.8.x (Orchard 1.8.x branch).

### Features:

- Notifications can be sent to user lists, roles or individual users.
- Notifications can contain tokens too, enabling dynamic notifications that e.g. include the current user's name.
- On the frontend notifications can be displayed through a widget in a configurable way. Users can hide notifications by closing them from the UI.

### Technical overview

A Notification Batch is what encapsulates all the data about recipients, the title and body of the notification. When "sending" a notification batch nothing really is being sent. When the Notification Batch gets published it only saves all the data required to dispatch notifications later.

Notifications actually "arrive" at users when the notifications that are sent to them are actually dispatched to them through the respective service. I.e. when this dispatching happens then (the last n) notification batches are checked and actual notifications generated from them where the user in question is among the recipients.

This means that we save processing when "sending" notifications but we trade it in for some processing when the user checks for new notifications. With this technique it's not necessary to do n operations for n recipients when sending out notifications (what would be necessary with using e.g. association records between users and notifications, which would get increasingly slower, and would eventually fail as the number of recipients grows).

Another trade-in is that this way it's only feasible to run limited (e.g. to most recent 50 notifications) "get notifications where this user is the recipient" queries.

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/orchard-user-notifications](https://bitbucket.org/Lombiq/orchard-user-notifications) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-User-Notifications](https://github.com/Lombiq/Orchard-User-Notifications) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/) for [RealtyShares](http://realtyshares.com/). Commercial-grade support is available through Lombiq.