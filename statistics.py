import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import copy

def read_file(n):
    data_person = pd.read_csv("People_{}.csv".format(n),names=["GroupID", "PersonID","Selflessness","Selfishness","Money","Strength","Power"])
    data_relation = pd.read_csv("Relationship_{}.csv".format(n), names = ["Subjet", "Target", "relation", "probability"])
    return data_person, data_relation

def read_entire_data(N):
    d1, d2 = read_file("2")
    people = [d1]
    relationship = [d2]
#    for i in range(1, N+1):
#        d1, d2 = read_file(i)
#        people.append(d1)
#        relationship.append(d2)
    return people, relationship

def grouping(data):
    average = dict()
    for i in range(len(data.index)):
        group = data.iloc[i,0]
        if group not in average.keys():
            average[group] = [0,0,0,0]
        average[group][0] += 1
        if data["Money"][i] != 0:
            average[group][1] += 1
        average[group][2] += int(data["Money"][i])
        average[group][3] += int(data["Power"][i])
    for i in average:
        average[i][2] = average[i][2]/average[i][0]
        average[i][3] = average[i][3]/average[i][0]
    return pd.DataFrame(average,index = ["Total", "Alive", "Money", "Power"]).transpose()

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
        average[person] += int(data_relation.iloc[i,2])
    return pd.DataFrame(average,index = ["average"]).transpose()

def power_relation(data_person, relation_average):
    power = data_person[["Power"]]
    data = pd.concat([power,relation_average],axis=1)
    return data

def count_alive(data_person):
    data = data_person.loc[data_person["Money"]!=0]
    return len(data["Money"])

    
def main():
    N = 1
    people, relationship = read_entire_data(N)
    people_group = []
    for i in people:
        people_group.append(grouping(i))
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
    
    
main()
