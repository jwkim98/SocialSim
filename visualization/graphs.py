import matplotlib.pyplot as plt
import seaborn as sns
import numpy as np
import pandas as pd
import copy

##@package graphs
#Convert Output csv file from the main model to pandas DataFrame
#Draw graph of values through time and scatter graph of several pairs of variables


##@method read_file
#read a csv file from given location
#input
#n : file number to read, location : location of files in string
#output
#data_people, data_relation : DataFrame
def read_file(n,location):
    data_people = pd.read_csv("{}People_{}.csv".format(location,n),names=["GroupID", "PersonID","Selflessness","Selfishness","Money","Strength","Power"])
    data_relation = pd.read_csv("{}Relationship_{}.csv".format(location,n), names = ["Subjet", "Target", "relation", "probability"])
    return data_people, data_relation

##@method read_entire_data
#read all required csv files from given location
#input
#N : number of files to read respectively for people and relationship
#location : location of files in string
#output
#people,relationship : list of DataFrame
def read_entire_data(N,location):
    people = []
    relationship = []
    for i in range(0, N):
        d1, d2 = read_file(i,location)
        people.append(d1)
        relationship.append(d2)
    return people, relationship

##@method entiregrouping
#make all people in one group and count several aspects
#input
#data : DataFrame with datas for people
#output
#DataFrame with number of people, number of people who are alive, average of Money, Power, Selfishness, Selflessness for living people
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

##@method time_based
#collect data based on time
#input
#data_set : list of DataFrame, name : label of data to collect
#output
#Dataframe with data collected based on time
def time_based(data_set, name):
    collection = {}
    for i in data_set[0].index:
        collection[i] = []
        for j in data_set:
            collection[i].append(j[name][i])
    return pd.DataFrame(collection)

##@method relation_average
#compute average of relationship among all people for each person
#input
#data_relation : DataFrame with datas of relationship
#output
#DataFrame with average of relationship for each person
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

##@method power_relation
#make DataFrame of power and average of relation for each person
#input
#data_people : DataFrame of datas of people, data_relation : DataFrame of datas of relationship
#output
#DataFrame of power and average of relation for each person
def power_relation(data_people, data_relation):
    relation=relation_average(data_relation)
    power = data_people[["Power"]]
    data = pd.concat([power,relation],axis=1)
    return data

def main(N, location):
    N = int(N)
    people, relationship = read_entire_data(N, location+"/")
    pow_rel = [power_relation(people[i],relationship[i]) for i in range(N)]
    people_group = []
    for i in people:
        people_group.append(entiregrouping(i))
    time_based_alive = time_based(people_group,"Alive")
    time_based_money = time_based(people_group,"Money")
    time_based_power = time_based(people_group,"Power")
    
    plt.figure(1, figsize = (9,12))
    plt.title("Time-based Features")
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
    
    fig1, axs1 = plt.subplots(2,(N//5+1)//2,figsize=(18,18),sharex=True,sharey=True)
    fig1.suptitle("Selfishness-Selflessness and Money")
    for i in range(0,N//5):
        j = 5*i+4
        axs1[i%2, i//2].scatter(people[j]["Selfishness"]-people[j]["Selflessness"],people[j]["Money"])
        axs1[i%2, i//2].set_title("epoch {}".format((i+1)*5))
        axs1[i%2, i//2].set(xlabel = "Selfishness-Selflessness",ylabel = "Money")
        axs1[i%2, i//2].label_outer()
    
    fig2, axs2 = plt.subplots(2,(N//5+1)//2,figsize=(18,18),sharex=True,sharey=True)
    fig2.suptitle("Power and Relation")
    for i in range(0,N//5):
        j = 5*i+4
        axs2[i%2, i//2].scatter(pow_rel[j]["Power"],pow_rel[j]["average"])
        axs2[i%2, i//2].set_title("epoch {}".format((i+1)*5))
        axs2[i%2, i//2].set(xlabel = "Power",ylabel = "Average of Relation")
        axs2[i%2, i//2].label_outer()
    
    fig3, axs3 = plt.subplots(2,(N//5+1)//2,figsize=(18,18),sharex=True,sharey=True)
    fig3.suptitle("Strength and Power")
    for i in range(0,N//5):
        j = 5*i+4
        axs3[i%2, i//2].scatter(people[j]["Strength"],people[j]["Power"])
        axs3[i%2, i//2].set_title("epoch {}".format((i+1)*5))
        axs3[i%2, i//2].set(xlabel = "Strength",ylabel = "Power")
        axs3[i%2, i//2].label_outer()

    fig4, axs4 = plt.subplots(2,(N//5+1)//2,figsize=(18,18),sharex=True,sharey=True)
    fig4.suptitle("Power and Money")
    for i in range(0,N//5):
        j = 5*i+4
        axs4[i%2, i//2].scatter(people[j]["Power"],people[j]["Money"])
        axs4[i%2, i//2].set_title("epoch {}".format((i+1)*5))
        axs4[i%2, i//2].set(xlabel = "Power",ylabel = "Money")
        axs4[i%2, i//2].label_outer()    

    fig5, axs5 = plt.subplots(2,(N//5+1)//2,figsize=(18,18),sharex=True,sharey=True)
    fig5.suptitle("Selfishness-Selflessness and Relation")
    for i in range(0,N//5):
        j = 5*i+4
        axs5[i%2, i//2].scatter(people[j]["Selfishness"]-people[j]["Selflessness"],pow_rel[j]["average"])
        axs5[i%2, i//2].set_title("epoch {}".format((i+1)*5))
        axs5[i%2, i//2].set(xlabel = "Selfishness-Selflessness",ylabel = "Average of Relation")
        axs5[i%2, i//2].label_outer()

    fig6, axs6 = plt.subplots(2,(N//5+1)//2,figsize=(18,18),sharex=True,sharey=True)
    fig6.suptitle("Relation and Money")
    for i in range(0,N//5):
        j = 5*i+4
        axs6[i%2, i//2].scatter(pow_rel[j]["average"], people[i]["Money"])
        axs6[i%2, i//2].set_title("epoch {}".format((i+1)*5))
        axs6[i%2, i//2].set(xlabel = "Selfishness-Selflessness",ylabel = "Average of Relation")
        axs6[i%2, i//2].label_outer()    
    plt.show()
    
    
    
    
if __name__=="__main__":
    N = input("Number of meeting cycle : ")
    location = input("Location of data : ")
    main(N, location)
