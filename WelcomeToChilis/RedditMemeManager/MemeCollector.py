#Python script called by c# to collect redit images
#Python 3+ version, compatible with iron python
import sys
import praw 
import random
import time 
import requests
import os, os.path

username = ""
password = ""
redditSecret = ""
redditId = ""
redditPassword = ""
userAgent = "Discord bot taking trending memes from reddit to send to server upon request"

redditPages = ["dankmemes", "me_irl"]
MemeFolderPath = ""
def ChooseRedditPage():
    randomIndex = random.randrange(len(redditPages))
    return redditPages[randomIndex]

def main():
    numImages = 10

    r = praw.Reddit(client_id = redditId, client_secret=redditSecret, user_agent=userAgent)


    while(True):
        if(len([name for name in os.listdir(MemeFolderPath) if os.path.isfile(os.path.join(MemeFolderPath, name))]) < 10):
            subreddit = r.subreddit(ChooseRedditPage()).hot(limit=numImages)
            for post in subreddit:
                url = (post.url)
                file_name = url.split("/")
                if len(file_name) == 0:
                    file_name = re.findall("/(.*?)", url)
                file_name = file_name[-1]
                if "." not in file_name:
                    file_name += ".jpg"
                print(file_name)

                if(file_name is not ".jpg"):
                    r = requests.get(url)
                    completePath = os.path.join(MemeFolderPath, file_name)
                    with open(completePath,"wb") as f:
                        #Save the content to local folder
                        f.write(r.content)

                print("Waiting two seconds to prevent api ban")
                time.sleep(2)

    time.sleep(60)

main()
