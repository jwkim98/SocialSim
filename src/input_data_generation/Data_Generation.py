
import csv
import numpy as np
looptime=3              #Group 개수
number=[100,100,100]          #각 Group당 사람수
number2=[100,100,100]         #각 Group당 사람수
strengthlimit=100       #최대 Strength
mean=[0,0,0]            #각 Group 당 사용할 Gaussian Distribution Mean
money=5000              #각 사람 당 Money(Group과 무관)
selfishvar=[1,1,1]      #각 Group 당 variance for selfishness
selflessvar=[1,1,1]     #각 Group 당 variance for selflessness
strvar=[1,1,1]          #각 Group 당 variance for strength
f=open('Personal data.csv','w')
write=csv.writer(f)
#Personal Data
for k in range(looptime):
    #Selfishness
    selfishness=np.random.normal(mean[k],selfishvar[k],number[k]) #Selfishness 뽑기
    minselfishness=min(selfishness)
    if (minselfishness<0):                           #만약 뽑은 데이터 중 음수가 있으면 모든 데이터를 양수로 만들기(Selfishness는 양수이므로)  
        selfishness=selfishness+abs(minselfishness)
    maxselfishness=max(selfishness)   
    selfishness=selfishness/maxselfishness                 #데이터 0~1로 Normalize
    selfishness=selfishness

    #Selflessness
    selflessness=np.random.normal(mean[k],selflessvar[k],number[k]) #Selflessness 뽑기
    minselflessness=min(selflessness)
    if (minselflessness<0):                           #만약 뽑은 데이터 중 음수가 있으면 모든 데이터를 양수로 만들기(Selflessness는 양수이므로)  
        selflessness=selflessness+abs(minselflessness)
    maxselflessness=max(selflessness)   
    selflessness=selflessness/maxselflessness                 #데이터 0~1로 Normalize
    selflessness=selflessness

    #Strength
    strength=np.random.normal(mean[k],strvar[k],number[k]) #strength 뽑기
    minstrength=min(strength)
    if (minstrength<0):                           #만약 뽑은 데이터 중 음수가 있으면 모든 데이터를 양수로 만들기(Strength는 양수이므로)  
        strength=strength+abs(minstrength)
    maxstrength=max(strength)   
    strength=strength/maxstrength                 #데이터 0~1로 Normalize
    strength=strength*strengthlimit
    


    #Group ID, Personal ID, 사람번호, 이타심, 이기심, money, strength 순
    for i in range(number[k]):
        if (k==0):
            personid=i
        else:
            personid=i
            for j in range(k):
                personid=personid+number[j]
        write.writerow(["%d" %0, "%d" %personid,"%.3f" %selflessness[i],"%.3f" %selfishness[i],"%d" %money,"%.3f" %strength[i]])
f.close()











#Relational Data
g=open('Relationship data.csv','w')
write=csv.writer(g)
meanforrelationship=0
relationvar=1  #variance for relationship
totalnumber=0
plus=relationvar  #plus for relationship between same relationship

for i in range(looptime):
    totalnumber=totalnumber+number[i]
numberforindex=number
for a in range(looptime):
    if (a!=0):
        numberforindex[a]=numberforindex[a]+number[a-1]
for i in range(totalnumber):
    #Make Relationship per Person
    PersonalRelationship=np.random.normal(meanforrelationship,relationvar,totalnumber-1)
    c=0
    groupnumber=0
    while(i>=numberforindex[c]):
        groupnumber=groupnumber+1
        c=c+1
    numberforplus=number2[groupnumber]
    for d in range(numberforplus-1):
        if (groupnumber==0):
            indexforplus=d
        else:
            indexforplus=numberforindex[groupnumber-1]+d
        PersonalRelationship[indexforplus]=PersonalRelationship[indexforplus]         #Better relationship
    maxRelation=abs(PersonalRelationship[0])
    for j in range(totalnumber-1):
        if(abs(PersonalRelationship[j])>maxRelation):
            maxRelation=abs(PersonalRelationship[j])
    PersonalRelationship=PersonalRelationship/maxRelation
    index=0

    # Opportunity to meet person
    for k in range(totalnumber):
        if (k!=i):
            if(groupnumber==0):
                if (k<number2[0]):
                    prob=0.05                      # 0.7
                    write.writerow(["%d" %i, "%d" %k, PersonalRelationship[index],"%.5f" %prob])
                    index=index+1
                else:
                    prob=0.05
                    write.writerow(["%d" %i, "%d" %k, PersonalRelationship[index],"%.5f" %prob])
                    index=index+1
            else:
                if (k>=number[groupnumber-1] and k<number[groupnumber]):
                    prob=0.05                      # 0.7
                    write.writerow(["%d" %i, "%d" %k, PersonalRelationship[index],"%.5f" %prob])
                    index=index+1
                else:
                    prob=0.05
                    write.writerow(["%d" %i, "%d" %k, PersonalRelationship[index],"%.5f" %prob])
                    index=index+1
g.close()

#확률 : Group 외는 0.05, Group 내는 0.7