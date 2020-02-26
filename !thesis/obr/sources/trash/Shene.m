function [N] = Shene(p,u,U)
%SHENE Summary of this function goes here
%   Detailed explanation goes here
m=length(U)-1;
n=m-p-1;
N=zeros(1,n+1);
if u==U(0+1)
    N(0+1)=1;
    return;
elseif u==U(m+1)
    N(n+1)=1;
    return;
end

k=FindSpan(p,u,U)+1;
N(k+1)=1;
for d=1:p
    N(k-d+1)=((U(k+1+1)-u)/(U(k+1+1)-U(k-d+1+1)))*N(k-d+1+1);
    for i=k-d+1:k-1
        N(i+1)=(u-U(i+1))/(U(i+d+1)-U(i+1))*N(i+1)+((U(i+d+1+1)-u)/(U(i+d+1+1)-U(i+1+1)))*N(i+1+1);
    end
    N(k+1)=(u-U(k+1))/(U(k+d+1)-U(k+1))*N(k+1);
end    
end

