< Data Generation >
Selfishness : mean 0, vairance 1인 Gaussian Distribution 생성 후 (0,1)로 Normailize
Selflessness : mean 0, vairance 1인 Gaussian Distribution 생성 후 (0,1)로 Normailize
Strength : mean 0, vairance 1인 Gaussian Distribution 생성 후 (0,strengthlimit(=100))로 Scaling
Money : 모든 사람 5000으로 초기화
Relationship : mean 0, vairance 1인 Gaussian Distribution 생성 후 (0,1)로 Normailize
가까운 정도 : Group 간에는 0.7(Group의 개념이 있다면), 아닌 경우는 0.05

<Data_Generation.py에 대한 설명>
Input : 없음
Output : Personal data.csv, Relationship data.csv가 생성된다. 
           Personal data.csv는 personal 정보를 담고 있고, Relationship data는 사람들간의 relationship 정보를 담고 있다.