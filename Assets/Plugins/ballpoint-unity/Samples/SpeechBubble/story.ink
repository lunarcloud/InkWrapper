VAR lastKnownSpeaker = "None"
LIST music = A, (B)

This is an example of narration.
* explore a bit[]. #hidedialog
  * * [done]Ok, now then... #showdialog
 * yes[... and?]?
-

And this is from the perspective of me. #speaker:A
It continues-

~music = A

Until I interrupt. #speaker:B
With choices even,
 * Yes[?].
  Well, okay then. # speaker:A
 * YAASSSS[!?]!
  And from that point onward A decided to... #speaker:None
  ** Never speak to B again
  ** Hang out with B from dusk til' dawn[] -> hang_out
  ** YAAAASSSSS[] # speaker:A
    *** lol
---
-> END

== hang_out ==
And so they totally did.

-> END