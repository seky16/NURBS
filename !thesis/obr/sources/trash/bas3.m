clc;clear;close;format;
U=[0 1 2 3];
m=length(U)-1;
p=2;
for u=U(1):0.1*U(m+1):U(m+1)
    if u==U(0+1)
        continue
    elseif u==U(m+1)
        continue
    end
    i=FindSpan(p,u,U)+1;
    N=BasisFuns(i,u,p,U)
end