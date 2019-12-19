import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import pandas as pd
import copy

def read_file(n,location):
    data_person = pd.read_csv("{}People_{}.csv".format(location,n),names=["GroupID", "PersonID","Selflessness","Selfishness","Money","Strength","Power"])
    data_relation = pd.read_csv("{}Relationship_{}.csv".format(location,n), names = ["Subjet", "Target", "relation", "probability"])
    return data_person, data_relation

def read_input_data(location):
    data_person = pd.read_csv("{}People.csv".format(location),names=["GroupID", "PersonID","Selflessness","Selfishness","Money","Strength"])
    data_relation = pd.read_csv("{}Relationship.csv".format(location), names = ["Subjet", "Target", "relation", "probability"])
    return data_person, data_relation
    


def read_entire_data(N,location):
    #    people, relationship = read_input_data(location+"input/")
    #    people=[people]
    #    relationship=[relationship]
    people = []
    relationship = []
    for i in range(0, N):
        d1, d2 = read_file(i,location)
        people.append(d1)
        relationship.append(d2)
    return people, relationship

def grouping(data):
    average = dict()
    for i in range(len(data.index)):
        group = data.iloc[i,0]
        if group not in average.keys():
            average[group] = [0,0,0,0,0,0]
        average[group][0] += 1
        if data["Money"][i] > 0:
            average[group][1] += 1
            average[group][2] += int(data["Money"][i])
            average[group][3] += int(data["Power"][i])
            average[group][4] += int(data["Selfishness"][i])
            average[group][5] += int(data["Selflessness"][i])
    for i in average:
        average[i][2] = average[i][2]/average[i][1]
        average[i][3] = average[i][3]/average[i][1]
        average[i][4] = average[i][4]/average[i][1]
        average[i][5] = average[i][5]/average[i][1]
    return pd.DataFrame(average,index = ["Total", "Alive", "Money", "Power","Selfishness","Selflessness"]).transpose()

def entiregrouping(data):
    average = dict()
    for i in range(len(data.index)):
        group = 0
        if group not in average.keys():
            average[group] = [0,0,0,0,0,0]
        average[group][0] += 1
        if data["Money"][i] > 0:
            average[group][1] += 1
            average[group][2] += int(data["Money"][i])
            average[group][3] += int(data["Power"][i])
            average[group][4] += int(data["Selfishness"][i])
            average[group][5] += int(data["Selflessness"][i])
    for i in average:
        average[i][2] = average[i][2]/average[i][1]
        average[i][3] = average[i][3]/average[i][1]
        average[i][4] = average[i][4]/average[i][1]
        average[i][5] = average[i][5]/average[i][1]
    return pd.DataFrame(average,index = ["Total", "Alive", "Money", "Power","Selfishness","Selflessness"]).transpose()


def time_based(data_set, name):
    collection = {}
    for i in data_set[0].index:
        collection[i] = []
        for j in data_set:
            collection[i].append(j[name][i])
    return pd.DataFrame(collection)




def relation_average(data_relation):
    average = dict()
    for i in range(len(data_relation.index)):
        person = data_relation.iloc[i,0]
        if person not in average.keys():
            average[person] = 0
        average[person] += float(data_relation.iloc[i,2])
    for i in average.keys():
        average[i] = average[i]/(len(average)-1)
    return pd.DataFrame(average,index = ["average"]).transpose()


def power_relation(data_person, data_relation):
    relation=relation_average(data_relation)
    power = data_person[["Power"]]
    data = pd.concat([power,relation],axis=1)
    return data

def count_alive(data_person):
    data = data_person.loc[data_person["Money"]!=0]
    return len(data["Money"])

def main(N, location):
    N = int(N)
    people, relationship = read_entire_data(N, location+"/")
    #people_input = people.pop(0)
    #relationship_input = relationship.pop(0)
    people_group = []
    for i in people:
        people_group.append(entiregrouping(i))
    time_based_alive = time_based(people_group,"Alive")
    time_based_money = time_based(people_group,"Money")
    time_based_power = time_based(people_group,"Power")
    plt.figure(1, figsize = (9,12))
    plt.subplot(311)
    plt.plot(time_based_alive)
    plt.ylabel("Alive")    
    plt.subplot(312)
    plt.plot(time_based_money)
    plt.ylabel("Money")    
    plt.subplot(313)
    plt.plot(time_based_power)
    plt.xlabel("Time")
    plt.ylabel("Power")
    plt.figure(2)
    plt.scatter(people[0]["Selfishness"],people[0]["Selflessness"])
    plt.xlabel("Selfishness")
    plt.ylabel("Selflessness")
    plt.figure(3)
    plt.scatter(people[-1]["Selfishness"]-people[-1]["Selflessness"],people[-1]["Money"])
    plt.xlabel("Selfishness-Selflessness")
    plt.ylabel("Money")
    plt.show()
    plt.figure(4)
    for i in range(N):
        #plt.figure(i+4)
        pow_rel = power_relation(people[i],relationship[i])
        plt.scatter(pow_rel["Power"],pow_rel["average"])
        plt.xlabel("Power")
        plt.ylabel("Average of relation")
    plt.figure(5)
    for i in range(N):
        plt.scatter(people[i]["Strength"],people[i]["Power"])
    
    
if __name__=="__main__":
    N = input("Number of meeting cycle : ")
    location = input("Location of data : ")
    main(N, location)