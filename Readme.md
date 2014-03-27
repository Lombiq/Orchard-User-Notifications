# Readme



This Orchard module adds the ability to send simple notifications to users and groups of users.


## Technical overview

A Notification Batch is what encapsulates all the data about recipients, the title and body of the notification. When "sending" a notification batch nothing really is being sent. When the Notification Batch gets published it only saves all the data required to dispatch notifications later.

Notifications actually "arrive" at users when the notifications that are sent to them are actually dispatched to them through the respective service. I.e. when this dispatching happens then (the last n) notification batches are checked and actual notifications generated from them where the user in question is among the recipients.

This means that we save processing when "sending" notifications but we trade it in for some processing when the user checks for new notifications. With this technique it's not necessary to do n operations for n recipients when sending out notifications (what would be necessary with using e.g. association records between users and notifications, which would get increasingly slower, and would eventually fail as the number of recipients grows).

Another trade-in is that this way it's only feasible to run limited (e.g. to most recent 50 notifications) "get notifications where this user is the recipient" queries.