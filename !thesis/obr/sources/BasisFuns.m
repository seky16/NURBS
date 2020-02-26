function [N] = BasisFuns(i,u,p,U)
%BASISFUNS Summary of this function goes here
%   Detailed explanation goes here
N(0+1)=1;
for j=1:p
    left(j+1)=u-U(i+1-j+1);
    right(j+1)=U(i+j+1)-u;
    saved=0;
    for r=0:(j-1)
        temp=N(r+1)/(right(r+1+1)+left(j-r+1));
        N(r+1)=saved+right(r+1+1)*temp;
        saved=left(j-r+1)*temp;
    end
    N(j+1)=saved;
end
end

