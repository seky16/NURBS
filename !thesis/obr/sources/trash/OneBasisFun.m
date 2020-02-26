function [Nip] = OneBasisFun(p,m,U,i,u)
%ONEBASISFUN Summary of this function goes here
%   Detailed explanation goes here
if ((i==0 && u==U(0+1)) || (i==m-p-1 && u==U(m+1)))
    Nip=1;
    return;
end
if (u<U(i+1) || u>=U(i+p+1+1))
    Nip=0;
    return;
end
for j=0:p
    if (u>=U(i+j+1) && u<U(i+j+1+1))
        N(j+1)=1;
    else
        N(j+1)=0;
    end
end
for k=1:p
    if N(0+1)==0
        saved=0;
    else
        saved=((u-U(i+1))*N(0+1))/(U(i+k+1)-U(i+1));
    end
    for j=0:p-k
        Uleft=U(i+j+1+1);
        Uright=U(i+j+k+1+1);
        if N(j+1+1)==0
            N(j+1)=saved;
            saved=0;
        else
            temp=N(j+1+1)/(Uright-Uleft);
            N(j+1)=saved+(Uright-u)*temp;
            saved=(u-Uleft)*temp;
        end
    end
end
Nip=N(0+1);
end

