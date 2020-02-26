function [ N ] = CBR( U,p,u )
%CBR Summary of this function goes here
%   Detailed explanation goes here

m=length(U)-1;
%n=m-p-1;
N=zeros(p+1);

%for u=U(1):0.01*U(m):U(m)
    for j=0:p
        for i=0:(p-j)
            if j==0
                if U(i+1)<=u && u<U(i+1+1)
                    N(i+1,j+1)=1;
                else
                    N(i+1,j+1)=0;
                end
            %elseif i+p+1>m
            %    N(i+1,j+1)=0;
            else
                if U(i+p+1)-U(i+1)==0
                    a=0;
                else
                    a=(u-U(i+1))/(U(i+p+1)-U(i+1));
                end
                if U(i+p+1+1)-U(i+1+1)==0
                    b=0;
                else
                    b=(U(i+p+1+1)-u)/(U(i+p+1+1)-U(i+1+1));
                end
                N(i+1,j+1)=a*N(i+1,p-1+1)+b*N(i+1+1,p-1+1);
            end
        end
    end
%end
end

