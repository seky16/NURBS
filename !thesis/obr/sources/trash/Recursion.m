function [N] = Recursion(p,U,u)
%RECURSION Summary of this function goes here
%   Detailed explanation goes here
m=length(U)+1;
n=m-p-1;

N=zeros(n+1,p+1);

for j=0:p
    for i=0:n
        if j==0
            if U(i+1)<=u && u<U(i+1+1)
                N(i+1,j+1)=1;
            else
                N(i+1,j+1)=0;
            end
        else
            N(i+1,j+1)=(u-U(i+1))/(U(i+p+1)-U(i+1))*N(i+1,j-1+1)+((U(i+j+1+1)-u)/(U(i+j+1+1)-U(i+1+1)))*N(i+1+1,j-1+1);
        end
    end
end
end