import matplotlib.pyplot as plt
import numpy as np
import pandas as pd

def read_file():
    data_person = pd.read_csv("data_person.csv",names=["PersonID","Selflessness","Selfishness","Money","Strength","Power"])
    data_relation = pd.read_csv("data_relation.csv", names = ["Subjet", "Target", "relation", "probability"])
    return data_person, data_relation

def read_file2():
    file1 = open("data_person.csv",'r')
    file2 = open("data_relation.csv",'r')
    data_person = file1.read().split()
    data_relation = file2.read().split()
    for i in range(len(data_person)):
        data_person[i] = data_person[i].split(",")
    for i in range(len(data_relation)):
        data_relation[i] = data_relation[i].split(",")
    data_person = pd.DataFrame(np.array(data_person),columns = ["PersonID","Selflessness","Selfishness","Money","Strength","Power"])
    data_relation = pd.DataFrame(np.array(data_relation), columns = ["Subjet", "Target", "relation", "probability"])
    return data_person, data_relation

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
    return power
    
def main():
    d1, d2 = read_file()
    print(d1)
    print(d2)
    d3 = relation_average(d2)
    print(d3)
    d4 = power_relation(d1, d3)
    print(d4)
main()