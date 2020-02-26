clc;clear;close;format;
U=[0 1 2 3];
m=length(U)-1;
for u=U(1):0.1*U(m+1):U(m+1)
    %N=CBR(U,2,u)
    N=Shene(2,u,U)
end    

%0.1*1+(2-0.1)*0