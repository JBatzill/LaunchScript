#settings filelink on
#settings filelink off
#launch "outlookmail://"
#snap left
#launch ""
#snap right
#browser new "www.netflix.de"
#snap left left
#browser new "www.amazon.de/Prime-Instant-Video/b?node=3279204031"
#snap right right
#snap left top top bottom right left left top top bottom top left top top left right right top
#launch http://www.google.de
#browser new www.google.de
#browser "www.youtube.de"
#browser new "www.youtube.de"
#launch "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
#snap right
#delay 3000
#launch "C:\Program Files (x86)\Notepad++\notepad++.exe"
#launch cmd
#snap left
#launch cmd "" "C:\cygwin64\"
#launch cmd
#launch "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"  "--profile-directory=Default --app-id=hmjkmjkepdijhoojdojkdfohbdgmmhki"
#launch "C:\cygwin64\Cygwin.bat" "" "C:\cygwin64\"
#launch "C:\cygwin64\bin\mintty.exe" "-i /Cygwin-Terminal.ico - ~" "C:\cygwin64\bin\"
#launch "C:\Program Files (x86)\Git\bin\sh.exe" "--login" "C:\Users\Johannes\SkyDrive\Dokumente\Projekte\PCR\PCR\"
#delay 2000

browser "www.google.de"
wait
delay 1000
type "hallo popelkopf" "{ENTER}"
type "{BS}"
delay 500
type "trolololololol"