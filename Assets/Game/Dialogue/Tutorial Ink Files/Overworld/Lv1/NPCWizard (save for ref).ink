VAR correctChoiceChosen = true

Welcome, chosen one. #Wizard

=== GiveTask ===
Welcome to the forest. You might not grasp the magnitude of the situation right now, but you have been chosen. #Wizard
I don't get it. Can you tell me how to get out of here? #Player
You will understand in time. #Wizard
For now, please help me find my necklace. I dropped it somewhere on the way over here. 
Help the wizard?
    * [Yes]
        Great!
        -> DONE
    * [No]
        Ooof...
        ~ correctChoiceChosen = false
        -> DONE
    * [Why would I do that?]
        Lol...
        ~ correctChoiceChosen = false
        -> DONE
        
-> END

=== RemoveItemTest ===
Removing item now!
-> END

=== StartTutorialBattle ===
I found it. #Player
Great job, I'm glad you can follow directions. #Wizard
However, can you hold your own in combat? Let's find out. #Wizard
Combat? Why would I do that? #Player
Stop talking, and follow me. #Wizard
-> END